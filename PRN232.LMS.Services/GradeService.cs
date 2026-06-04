using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Models;
using PRN232.LMS.Repositories.Extensions;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.Models;
using System.Linq.Expressions;

namespace PRN232.LMS.Services
{
    public class GradeService(IGradeRepository _gradeRepository) : IGradeService
    {
        public GradeModel GetGradeById(int id)
        {
            Grade g = _gradeRepository.GetGradeById(id);
            return new GradeModel
            {
                GradeId = g.GradeId,
                EnrollmentId = g.EnrollmentId,
                SubjectId = g.SubjectId,
                MidtermScore = g.MidtermScore,
                FinalScore = g.FinalScore,
                TotalScore = g.TotalScore,
                LetterGrade = g.LetterGrade
            };
        }

        public async Task<PagedResult<dynamic>> GetGradesAsync(QueryParameters queryParams)
        {
            Expression<Func<Grade, bool>>? searchFilter = null;
            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                searchFilter = g => g.LetterGrade.Contains(queryParams.Search) ||
                                    g.Subject.SubjectName.Contains(queryParams.Search) ||
                                    g.Subject.SubjectCode.Contains(queryParams.Search);
            }

            var (grades, totalCount) = await _gradeRepository
                .GetCollectionAsync(queryParams.Expand, queryParams.Sort, queryParams.Page, queryParams.Size, searchFilter);

            var businessModels = grades.Select(g => new GradeModel
            {
                GradeId = g.GradeId,
                EnrollmentId = g.EnrollmentId,
                SubjectId = g.SubjectId,
                MidtermScore = g.MidtermScore,
                FinalScore = g.FinalScore,
                TotalScore = g.TotalScore,
                LetterGrade = g.LetterGrade
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
    }
}
