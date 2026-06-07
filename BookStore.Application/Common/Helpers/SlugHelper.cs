namespace BookStore.Application.Common.Helpers
{
    public static class SlugHelper
    {
        public static string GenerateSlug(string value)
        {
            return value
                .Trim()
                .ToLower()
                .Replace(" ", "-");
        }
    }
}
