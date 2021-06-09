namespace AzureCognitiveSearch.Pages
{
    public class SearchResultViewModel
    {
        //public string filename { get; set; }
        //public string filepath { get; set; }
        //public string filetext { get; set; }
        //public string highlightedtext { get; set; }


        public int Id { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string Tags { get; set; }
        public bool IsActive { get; set; }
        public string highlightedtext { get; set; }
        public string Language { get; set; }
        public int ArticleId { get; set; }
    }
}