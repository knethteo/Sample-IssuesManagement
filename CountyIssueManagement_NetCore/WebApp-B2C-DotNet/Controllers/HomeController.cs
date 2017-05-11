using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using WebApp_OpenIDConnect_DotNet_B2C.DatabaseContext;
using WebApp_OpenIDConnect_DotNet_B2C.Models;

namespace WebApp_OpenIDConnect_DotNet_B2C.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var ctx = new IssueManagementDbContext())
            {
                Issue issue = new Issue()
                {
                    
                    Description="",

                    


                    IssueFieldAgent = new IssueFieldAgent()
                    {
                        Name = "Mimi"
                                            }


                    
                    
                };
                ctx.Issues.Add(issue); // Create
                                       //ctx.Issues.ToList(); // List all
                                       //ctx.Issues.Find(1); // Read
                                       //ctx.Entry(issue).State = EntityState.Modified; // Update
                                       //ctx.Entry(issue).State = EntityState.Deleted; // Delete

                //ctx.SaveChanges();

                test4Kenneth kenneth = new test4Kenneth()
                {
                    Title = "Hello"
                };

                test4Jagan test = new test4Jagan()
                {
                   // Id = 01,
                    Title = "Jagan"
                   
                };


            }

            return View();
        }

        // You can use the PolicyAuthorize decorator to execute a certain policy if the user is not already signed into the app.
        [Authorize]
        public ActionResult Claims()
        {
            Claim displayName = ClaimsPrincipal.Current.FindFirst(ClaimsPrincipal.Current.Identities.First().NameClaimType);
            
            ViewBag.DisplayName = displayName != null ? displayName.Value : string.Empty;
            return View();
        }

        public ActionResult Error(string message)
        {
            ViewBag.Message = message;

            return View("Error");
        }
    }
}