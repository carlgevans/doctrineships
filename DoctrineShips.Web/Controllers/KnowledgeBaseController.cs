namespace DoctrineShips.Web.Controllers
{
    using System.Web.Mvc;
    using System.Web.UI;
    using DoctrineShips.Service;
    using DoctrineShips.Web.ViewModels;
    using Tools;

    [Authorize]
    public class KnowledgeBaseController : Controller
    {
        private readonly IDoctrineShipsServices doctrineShipsServices;

        public KnowledgeBaseController(IDoctrineShipsServices doctrineShipsServices)
        {
            this.doctrineShipsServices = doctrineShipsServices;
        }

        public ActionResult List()
        {
            KnowledgeBaseListViewModel viewModel = new KnowledgeBaseListViewModel();

            viewModel.Articles = this.doctrineShipsServices.GetArticles();

            return View(viewModel);
        }

        
        public ActionResult Article(string articleId)
        {
            // Cleanse the passed article id string to prevent XSS.
            int cleanArticleId = Conversion.StringToInt32(Server.HtmlEncode(articleId));

            KnowledgeBaseArticleViewModel viewModel = new KnowledgeBaseArticleViewModel();

            viewModel.Article = this.doctrineShipsServices.GetArticle(cleanArticleId);

            return View(viewModel);
        }
    }
}
