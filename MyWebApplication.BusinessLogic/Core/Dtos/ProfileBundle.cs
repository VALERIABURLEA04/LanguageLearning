using System.Collections.Generic;

namespace MyWebApplication.BusinessLogic.Core.Dtos
{
    public record ProfileBundle(
        UserDto User,
        IReadOnlyList<PurchaseDto> Purchases,
        IReadOnlyList<CourseDto> EnrolledCourses);
}
