using Microsoft.AspNetCore.Mvc;
using MyWebApplication.BusinessLogic.Command;
using MyWebApplication.BusinessLogic.Core.Dtos;
using sa.Models;

namespace Controllers.Api;

[ApiController]
[Route("api/checkout")]
[Produces("application/json")]
public class CheckoutApiController : ControllerBase
{
    private readonly CheckoutCommand _checkout;

    public CheckoutApiController(CheckoutCommand checkout)
    {
        _checkout = checkout;
    }

    /// <summary>Lists all available course categories with prices.</summary>
    [HttpGet("categories")]
    public ActionResult<IEnumerable<object>> ListCategories() =>
        Ok(CourseCategory.All.Select(c => new
        {
            c.Slug,
            c.Title,
            c.Level,
            c.Duration,
            c.Frequency,
            c.Price,
            c.OldPrice,
            c.PriceUnit,
            c.DiscountPercent
        }));

    /// <summary>Gets details for a single course category by slug.</summary>
    [HttpGet("categories/{slug}")]
    public ActionResult<CourseCategory> GetCategory(string slug)
    {
        var cat = CourseCategory.FindBySlug(slug);
        return cat is null ? NotFound(new { error = $"Categoria '{slug}' nu a fost găsită." }) : Ok(cat);
    }

    /// <summary>
    /// Submits a checkout for a course. Returns the receipt and the charged amount.
    /// </summary>
    /// <remarks>
    /// Example body:
    /// {
    ///   "categorySlug": "adulti",
    ///   "fullName": "Maria Popescu",
    ///   "email": "maria@test.com",
    ///   "phone": "+373 60 000 000",
    ///   "paymentMethod": "stripe",
    ///   "plan": "full"
    /// }
    /// </remarks>
    [HttpPost]
    public ActionResult<CheckoutResult> Submit([FromBody] CheckoutApiInput input)
    {
        var category = CourseCategory.FindBySlug(input.CategorySlug);
        if (category is null)
            return NotFound(new { error = $"Categoria '{input.CategorySlug}' nu a fost găsită." });

        _checkout.Setup(new CheckoutRequest
        {
            CategorySlug  = category.Slug,
            CourseTitle   = category.Title,
            FullName      = input.FullName,
            Email         = input.Email,
            Phone         = input.Phone,
            PaymentMethod = input.PaymentMethod,
            Plan          = input.Plan,
            TotalAmount   = category.Price
        });
        _checkout.Execute();
        var result = _checkout.Result;

        return result.Success ? Ok(result) : BadRequest(result);
    }
}

public class CheckoutApiInput
{
    public string CategorySlug  { get; set; } = string.Empty;
    public string FullName      { get; set; } = string.Empty;
    public string Email         { get; set; } = string.Empty;
    public string Phone         { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = "stripe";
    public string Plan          { get; set; } = "full";
}
