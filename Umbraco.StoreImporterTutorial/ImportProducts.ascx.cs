using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Umbraco.Core.Models;
using Umbraco.Web;
using umbraco;
using File = System.IO.File;

namespace Umbraco.StoreImporterTutorial
{
    // We inherit from the Umbraco.Web.UmbracoUserControl which gives us access to various Umbraco services and parameters
    public partial class ImportProducts : UmbracoUserControl
    {
        private List<KeyValuePair<int, int>> _productGroups;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void startImport_Click(object sender, EventArgs e)
        {
            PanelUpload.Visible = false;

            if (ProductFile.HasFile)
            {
                string tempFile = Server.MapPath(string.Format("~/App_Data/products{0}.csv", Guid.NewGuid()));

                try
                {
                    int count = 0;
                    ProductFile.PostedFile.SaveAs(tempFile);

                    var lines = File.ReadLines(tempFile).Select(a => a.Split(';'));
                    foreach (var product in lines)
                    {
                        // first row is just headers, we'll by pass it
                        if (count > 0)
                        {
                            ImportProduct(
                                int.Parse(product[0]),
                                product[1],
                                int.Parse(product[2]),
                                int.Parse(product[3]));
                        }
                        count++;

                    }

                    ProductCount.Text = count.ToString();
                    PanelWoot.Visible = true;
                    PanelFail.Visible = false;
                }
                catch (Exception ee)
                {
                    failDetail.Text = ee.ToString();
                    PanelWoot.Visible = false;
                    PanelFail.Visible = true;
                }
                finally
                {
                    if (File.Exists(tempFile))
                        File.Delete(tempFile);
                }
            }
        }

        private void ImportProduct(int productId, string name, int productGroup, int price)
        {
            // Get the Umbraco Content Service
            var contentService = Services.ContentService;

            // get the Umbraco Node id of the product group
            var productGroupNodeId = getNodeIdFromProductGroupId(productGroup);

            if (productGroupNodeId > 0)
            {
                // We'll have a local variable for our product as we'll first see if it exist (so we can update it), else we'll create it
                IContent product;

                // Use uQuery to see if a node of type "product" exists with the productId from our ERP
                var productNode =
                    uQuery.GetNodesByType("product").FirstOrDefault(x => x.GetProperty("productId").Value.ToString() == productId.ToString());
                if (productNode != null)
                {
                    // We can use the ContentService to get an existing node via its Id
                    product = contentService.GetById(productNode.Id);

                    // maybe the product has moved to another category - then we'll move it
                    if (product.ParentId != productGroupNodeId)
                        contentService.Move(product, productGroupNodeId);
                }
                else
                {

                    product = contentService.CreateContent(
                        name, // the name of the product 
                        productGroupNodeId, // the parent id should be the id of the group node 
                        "product", // the alias of the product Document Type
                        0
                        );
                }

                // We need to update properties (product id, original name and the price)
                product.SetValue("productId", productId);
                product.SetValue("originalName", name);
                product.SetValue("priceDKK", price);

                // finally we need to save and publish it (which also saves the product!) - that's done via the Content Service
                contentService.SaveAndPublish(product);

                // we could also just save the product and not publish it, by calling the Save method of the contentService:
                // contentService.Save(product);
            }
            else
                throw new ArgumentException(string.Format("Couldn't find a product group node that had the id of {0}",
                                                          productGroup));
        }

        private int getNodeIdFromProductGroupId(int groupId)
        {
            if (_productGroups == null)
            {
                // we need to find all nodes that's a product group and get their product group ids for lookups
                _productGroups = new List<KeyValuePair<int, int>>();
                foreach (var group in uQuery.GetNodesByType("productGroup"))
                {
                    _productGroups.Add(new KeyValuePair<int, int>(
                        group.Id,
                        int.Parse(group.GetProperty("groupId").Value)));
                }
            }

            return _productGroups.Find(x => x.Value == groupId).Key;

        }
    }
}