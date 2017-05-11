using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApp_OpenIDConnect_DotNet_B2C.DatabaseContext;
using WebApp_OpenIDConnect_DotNet_B2C.Models;
using System.Security.Claims;
using System.Web.Security;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.Caching;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApp_OpenIDConnect_DotNet_B2C.General;
using System.Web.Configuration;

namespace WebApp_OpenIDConnect_DotNet_B2C.Controllers
{
    [Authorize]
        public class IssuesController : Controller
    {
        private IssueManagementDbContext db = new IssueManagementDbContext();
        
        // GET: Issues

        //[Authorize(Roles = "officer")]
        public async Task<ActionResult> Index()
        {
            var isCitizen = false;
            ClaimsIdentity cid = ((ClaimsIdentity)User.Identity);
            var email = cid.Claims.Where(c => c.Type == "emails").Select(c => c.Value).SingleOrDefault();
            if (cid.ToString() != string.Empty)
            {
                if (email.ToString().Contains("@microsoft.com") == false)
                {
                    isCitizen = true;
                }
            }

            var issues = db.Issues.Include(i => i.IssueCategory).Include(i => i.IssueStatus).Include(i => i.IssuePriority);
            if (isCitizen)
                issues = issues.Where(i => i.Creator == email);

            issues = issues.OrderBy(i => i.IssuePriority.Id);

            ViewBag.isCitizen = isCitizen;
            return View(await issues.ToListAsync());
        }

        // GET: Issues/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            var isCitizen = false;
            ClaimsIdentity cid = ((ClaimsIdentity)User.Identity);
            var email = cid.Claims.Where(c => c.Type == "emails").Select(c => c.Value).SingleOrDefault();
            if (cid.ToString() != string.Empty)
            {
                if (email.ToString().Contains("@microsoft.com") == false)
                {
                    isCitizen = true;
                }
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var issues = db.Issues.
                Include(i => i.IssueCategory).
                Include(i => i.IssuePriority).
                Include(i => i.IssueStatus).
                Include(i => i.IssueFieldAgent);

            Issue issue = issues.FirstOrDefault(i => i.Id == id);
            if (issue == null)
            {
                return HttpNotFound();
            }
            ViewBag.isCitizen = isCitizen;
            ViewBag.ImageUrl = issue.ImageUrl;
            return View(issue);
        }

        // GET: Issues/Create
        [AllowAnonymous]
        public ActionResult Create()
        {
            var isCitizen = false;
            ClaimsIdentity cid = ((ClaimsIdentity)User.Identity);
            var email = cid.Claims.Where(c => c.Type == "emails").Select(c => c.Value).SingleOrDefault();
            if (cid.ToString() != string.Empty)
            {
                if (email.ToString().Contains("@microsoft.com") == false)
                {
                    isCitizen = true;
                }
            } 

            ViewBag.IssueCategoryId = new SelectList(db.IssueCategories, "Id", "Name");
            ViewBag.IssueStatusId = new SelectList(db.IssueStatuses, "Id", "Name");
            ViewBag.IssuePriorityId = new SelectList(db.IssuePriorities, "Id", "Name");
            ViewBag.IssueFieldAgentId = new SelectList(db.IssueFieldAgents, "Id", "Name");
            ViewBag.isCitizen = isCitizen;
            ViewBag.email = email;
            if (TempData["imguri2"] == null)
            {
                ViewBag.imguri = "Waiting for upload";
            }
            else
            ViewBag.imguri = TempData["imguri2"].ToString();
            return View();
        }

        // POST: Issues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,IssueCategoryId,IssuePriorityId,Location,Latitude,Longitude,Description,Feedback,IssueStatusId,Eta,Creator,InternalComment,IssueFieldAgentId,CreationTime,CompletionTime,ImageUrl")] Issue issue)
        {
            var isCitizen = false;
            ClaimsIdentity cid = ((ClaimsIdentity)User.Identity);
            var email = cid.Claims.Where(c => c.Type == "emails").Select(c => c.Value).SingleOrDefault();
            if (cid.ToString() != string.Empty)
            {
                if (email.ToString().Contains("@microsoft.com") == false)
                {
                    isCitizen = true;
                }
            }

            if (ModelState.IsValid)
            {
                //issue.IssuePriority = new IssuePriority() { Name = "Medium" };
                issue.CreationTime = DateTime.Now;
                db.Issues.Add(issue);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.IssueCategoryId = new SelectList(db.IssueCategories, "Id", "Name");
            ViewBag.IssueStatusId = new SelectList(db.IssueStatuses, "Id", "Name");
            ViewBag.IssuePriorityId = new SelectList(db.IssuePriorities, "Id", "Name");
            ViewBag.IssueFieldAgentId = new SelectList(db.IssueFieldAgents, "Id", "Name");
            ViewBag.isCitizen = isCitizen;
            return RedirectToAction("FileUpload");
        }

        // GET: Issues/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            ClaimsIdentity cid = ((ClaimsIdentity)User.Identity);
            var email = cid.Claims.Where(c => c.Type == "emails").Select(c => c.Value).SingleOrDefault();
            var isCitizen = !email.ToString().Contains("@microsoft.com");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Issue issue = await db.Issues.FindAsync(id);
            if (issue == null)
            {
                return HttpNotFound();
            }
            ViewBag.IssueCategoryId = new SelectList(db.IssueCategories, "Id", "Name", issue.IssueCategoryId);
            ViewBag.IssueStatusId = new SelectList(db.IssueStatuses, "Id", "Name", issue.IssueStatusId);
            ViewBag.IssuePriorityId = new SelectList(db.IssuePriorities, "Id", "Name", issue.IssuePriorityId);
            ViewBag.IssueFieldAgentId = new SelectList(db.IssueFieldAgents, "Id", "Name", issue.IssueFieldAgentId);
            ViewBag.isCitizen = isCitizen;
            return View(issue);
        }

        // POST: Issues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,IssueCategoryId,IssuePriorityId,Location,Latitude,Longitude,Description,Feedback,IssueStatusId,Eta,Creator,InternalComment,IssueFieldAgentId,CreationTime,CompletionTime,ImageUrl")] Issue issue)
        {
            try
            {
                ClaimsIdentity cid = ((ClaimsIdentity)User.Identity);
                var email = cid.Claims.Where(c => c.Type == "emails").Select(c => c.Value).SingleOrDefault();
                var isCitizen = !email.ToString().Contains("@microsoft.com");

                if (ModelState.IsValid)
                {
                    if (issue.IssueStatusId == 6)
                        issue.CompletionTime = DateTime.Now;

                    db.Entry(issue).State = EntityState.Modified;
                    await UpdateSentiment(issue.Id, Request.Form[6].ToString());

                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                    //return Content("Done");

                }


                ViewBag.IssueCategoryId = new SelectList(db.IssueCategories, "Id", "Name");
                ViewBag.IssueStatusId = new SelectList(db.IssueStatuses, "Id", "Name");
                ViewBag.IssuePriorityId = new SelectList(db.IssuePriorities, "Id", "Name");
                ViewBag.IssueFieldAgentId = new SelectList(db.IssueFieldAgents, "Id", "Name");
                ViewBag.isCitizen = isCitizen;
                return View(issue);
            }
            catch (Exception e)
            {
                return View(issue);
            }
        }
        private const string BaseUrl = "https://westus.api.cognitive.microsoft.com/";
        private string AccountKey = WebConfigurationManager.AppSettings["TextAnalyticsKey"].ToString();
        

        //with void, can get sentiment, but throw exception
        private async Task UpdateSentiment(int id, string Feedback)
        {
            try {
                //int idx = Convert.ToInt32(id);

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseUrl);

                    // Request headers.
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", AccountKey);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Request body. Insert your text data here in JSON format.
                    byte[] byteData = Encoding.UTF8.GetBytes("{\"documents\":[" +
                        "{\"id\":\"1\",\"text\":\"" + Feedback + "\"}]}");

                    var uri = "text/analytics/v2.0/sentiment";
                    var response = await CallEndpoint(client, uri, byteData);

                    //"{\"documents\":[{\"score\":0.5,\"id\":\"1\"}],\"errors\":[]}"
                    dynamic stuff = JsonConvert.DeserializeObject(response);
                    float sentimentScore = stuff.documents[0].score;

                    
                        var result = db.Issues.SingleOrDefault(b => b.Id == id);
                        if (result != null)
                        {
                            result.Feedback = sentimentScore + " ," + Feedback;

                        }

                        await db.SaveChangesAsync();


                    
                }
                

                    //var result = (Sentiment)JsonConvert.DeserializeObject(response, typeof(Sentiment));

                    //string x = result.docs[0].ToString();


                    //string sentimentScore = x;
                    //string r = response;
                    //Console.WriteLine("\nDetect sentiment response:\n" + response);
                }
            catch (Exception e)
            { }
        }

        static async Task<String> CallEndpoint(HttpClient client, string uri, byte[] byteData)
        {
            try
            {
                using (var content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var response = await client.PostAsync(uri, content);
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {
                return "";
            }
        }

        // GET: Issues/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            ClaimsIdentity cid = ((ClaimsIdentity)User.Identity);
            var email = cid.Claims.Where(c => c.Type == "emails").Select(c => c.Value).SingleOrDefault();
            var isCitizen = !email.ToString().Contains("@microsoft.com");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Issue issue = await db.Issues.FindAsync(id);
            if (issue == null)
            {
                return HttpNotFound();
            }
            ViewBag.isCitizen = isCitizen;
            return View(issue);
        }

        // POST: Issues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Issue issue = await db.Issues.FindAsync(id);
            db.Issues.Remove(issue);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult FileUpload(Issue currentIssue, HttpPostedFileBase UploadFile)
        {
            Guid id = Guid.NewGuid();
            string accountname = "countyim";
            string accesskey = "xYEWcULG42D5QlvK8Oi5Tt2L/Lpv1mF8YowjDAnyCZt4/zb81Fb9JNcJozETQraNXAhP//ZP98DOi4fmuEFjqw==";
            StorageCredentials cred = new StorageCredentials(accountname, accesskey);
            CloudStorageAccount storageAccount = new CloudStorageAccount(cred, useHttps: true);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            //retrieve ref to container
            CloudBlobContainer container = blobClient.GetContainerReference("issuesimages");

            container.CreateIfNotExists();
            container.SetPermissions(
                new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Container
                });

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(id.ToString()+".jpg");
            MemoryStream ms = new MemoryStream();
            Image img = Image.FromStream(UploadFile.InputStream);
            img.Save(ms, ImageFormat.Jpeg);
            ms.Position = 0;
            blockBlob.UploadFromStream(ms);
            string imguri2 = blockBlob.StorageUri.PrimaryUri.ToString();

            TempData["imguri2"] = imguri2;
            // after successfully uploading redirect the user
            return RedirectToAction("Create");

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
