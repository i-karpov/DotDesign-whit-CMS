
namespace DotDesign.WebUI.Utility
{
    /// <summary>
    /// Used to inject base paging setting for CRUD controllers.
    /// </summary>
    public class PagingSettings
    {
        /// <summary>
        /// Is used to specify first page.
        /// </summary>
        public const int FirstPageNumber = 1;

        /// <summary>
        /// Defines maximum count of items on single page.
        /// </summary>
        public int ItemsPerPageCount
        {
            get;
            set;
        }

        public PagingSettings(int itemsPerPageCount)
        {
            this.ItemsPerPageCount = itemsPerPageCount;
        }

        /// <summary>
        /// Ensures that supposed page number is in correct range,
        /// and if needed replaces it with appropriate default value:
        /// If given value is null, less than FirstPageNumber constant
        /// or greater than pages count, then last page id will be returned.
        /// </summary>
        /// <param name="supposedPageNumber">
        /// Defines desired page number.
        /// </param>
        /// <param name="totalItemsCount">
        /// Total count of items.
        /// </param>
        /// <returns>Correct page number.</returns>
        public int EnsurePageNumber(int? supposedPageNumber, int totalItemsCount)
        {
            int correctPageNumber;
            int pagesCount = (totalItemsCount + ItemsPerPageCount - 1) / ItemsPerPageCount;
            if (pagesCount < FirstPageNumber)
            {
                pagesCount = FirstPageNumber;
            }
            if (!supposedPageNumber.HasValue ||
                    supposedPageNumber < FirstPageNumber ||
                    supposedPageNumber > pagesCount)
            {
                correctPageNumber = pagesCount;
            }
            else
            {
                correctPageNumber = supposedPageNumber.Value;
            }
            return correctPageNumber;
        }
    }
}