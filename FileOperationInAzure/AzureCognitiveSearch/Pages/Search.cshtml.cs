using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace AzureCognitiveSearch.Pages
{
    public class SearchModel : PageModel
    {


        public void OnGet()
        {
          
            //coginitve search name
            string SearchServiceName = "cogsearchservice";
            //TODO - Add your admin key
            string SearchServiceAdminKey = "Add your admin key";

            string searchdata = "*";//"రాజమౌళి";
            //indexer name - colletion dbs
            string indexname = "azuresql-index1";

           // search = *&$count = true &$select = metadata_storage_name,persons,organizations,locations,languageCode

            //SearchServiceClient searchclient = new SearchServiceClient(SearchServiceName, new SearchCredentials(SearchServiceAdminKey));

            // ISearchIndexClient IndexClient = searchclient.Indexes.Get(indexname);

            try
            {
                //Uri serviceEndpoint = new Uri($"https://{SearchServiceName}.search.windows.net/");
                //connection establishment
                SearchIndexClient client = new SearchIndexClient(SearchServiceName, indexname, new SearchCredentials(SearchServiceAdminKey));

                SearchParameters parameters = new SearchParameters();

                //parameters.HighlightFields = new List<string> { "Title" , "Description","Tags" };
                parameters.HighlightFields = new List<string> {  "Description"};
                parameters.HighlightPreTag = "<b>";
                parameters.HighlightPostTag = "</b>";
                parameters.IncludeTotalResultCount = true;
                parameters.Top = 2;
                parameters.Skip = 3;

                //parameters.Top = 2;
                //parameters.QueryType=


                var result = client.Documents.SearchAsync(searchdata,parameters).Result;

                IList<SearchResultViewModel> seachresults = new List<SearchResultViewModel>();
                foreach (var data in result.Results)
                {
                    SearchResultViewModel currentdata = new SearchResultViewModel();
                    currentdata.Id = Convert.ToInt32(data.Document["Id"]);
                    currentdata.Title = data.Document["Title"].ToString();
                    currentdata.Description = data.Document["Description"].ToString();
                    currentdata.Tags = data.Document["Tags"].ToString();
                    currentdata.IsActive = Convert.ToBoolean(data.Document["IsActive"].ToString());
                    currentdata.ArticleId = Convert.ToInt32(data.Document["AriticalId"]);
                    currentdata.Language= data.Document["Language"].ToString();

                    if (data.Highlights != null)
                    {
                        //foreach (var item in data.Highlights)
                        //{

                        //    currentdata.highlightedtext = data.Highlights[item.Key].ToString();
                        //    foreach (var high in data.Highlights[item.Key].ToList())
                        //    {
                        //        currentdata.highlightedtext += high;
                        //    }

                        //}
                        currentdata.highlightedtext = data.Highlights["Description"].ToString();
                        foreach (var high in data.Highlights["Description"].ToList())
                        {
                            currentdata.highlightedtext += high;
                        }
                    }
                       seachresults.Add(currentdata);

                }



                #region 
                //  IList<SearchResultViewModel> seachresults = new List<SearchResultViewModel>();
                //foreach (var data in result.Results)
                //{

                //    SearchResultViewModel currentdata = new SearchResultViewModel();

                //    currentdata.filename = data.Document["metadata_storage_name"].ToString();

                //    var path = data.Document["metadata_storage_path"].ToString();
                //    path = path.Substring(0, path.Length - 1);

                //    var bytedata = WebEncoders.Base64UrlDecode(path);
                //    currentdata.filepath = System.Text.ASCIIEncoding.ASCII.GetString(bytedata);

                //    currentdata.filetext = data.Document["merged_content"].ToString();

                //    if (data.Highlights != null)
                //    {

                //        //currentdata.highlightedtext = data.Highlights["merged_content"].ToString();
                //        foreach (var high in data.Highlights["merged_content"].ToList())
                //        {
                //            currentdata.highlightedtext += high;
                //        }
                //    }
                //    seachresults.Add(currentdata);
                //}
                #endregion


            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }
                 }
    }
}
