using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Repositories.Models;
using PRN232.LMS.Repositories.Extensions;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.Models;
using System.Linq.Expressions;

namespace PRN232.LMS.Services
{
    public class SubjectService(ISubjectRepository _subjectRepository) : ISubjectService
    {
        public SubjectModel GetSubjectById(int id)
        {
            Subject s = _subjectRepository.GetSubjectById(id);
            return new SubjectModel
            {
                SubjectId = s.SubjectId,
                SubjectCode = s.SubjectCode,
                SubjectName = s.SubjectName,
                Credit = s.Credit
            };
        }

        public async Task<PagedResult<dynamic>> GetSubjectsAsync(QueryParameters queryParams)
        {
            Expression<Func<Subject, bool>>? searchFilter = null;
            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                searchFilter = s => s.SubjectName.Contains(queryParams.Search) ||
                                    s.SubjectCode.Contains(queryParams.Search);
            }

            var (subjects, totalCount) = await _subjectRepository
                .GetCollectionAsync(queryParams.Expand, queryParams.Sort, queryParams.Page, queryParams.Size, searchFilter);

            var businessModels = subjects.Select(s => new SubjectModel
            {
                SubjectId = s.SubjectId,
                SubjectCode = s.SubjectCode,
                SubjectName = s.SubjectName,
                Credit = s.Credit
            }).ToList();

            var shaped = businessModels.ShapeData(queryParams.Fields);

            return new PagedResult<dynamic>
            {
                Items = shaped,
                Pagination = new PaginationMetadata
                {
                    Page = queryParams.Page,
                    PageSize = queryParams.Size,
                    TotalItems = totalCount,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)queryParams.Size)
                }
            };
        }

        public SubjectModel CreateSubject(SubjectModel model)
        {
            // Check if SubjectCode already exists in the database
            var codeExists = _subjectRepository.GetSubjects().Any(s => s.SubjectCode.Equals(model.SubjectCode, StringComparison.OrdinalIgnoreCase));
            if (codeExists)
            {
                throw new InvalidOperationException($"Subject Code '{model.SubjectCode}' already exists.");
            }

            var subject = new Subject
            {
                SubjectCode = model.SubjectCode,
                SubjectName = model.SubjectName,
                Credit = model.Credit
            };

            _subjectRepository.AddSubject(subject);

            model.SubjectId = subject.SubjectId;
            return model;
        }
    }
}
