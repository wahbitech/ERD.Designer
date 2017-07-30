using Drawer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Drawer
{
    public partial class DrawingPage : System.Web.UI.Page
    {
        public string cstr = @"Data Source=localhost;Initial Catalog=DrawerDb;Integrated Security=True";
        public static List<EntityModel> entities = new List<EntityModel>();
        public static List<AttributeModel> attributes = new List<AttributeModel>();
        public static List<RelationshipModel> relationships = new List<RelationshipModel>();
        public static List<CompositeChildAttributeModel> compositeAttributeChilds = new List<CompositeChildAttributeModel>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ddlSavedDiagramName.Items.Clear();

                var lst = listSavedDiagrams();
                ddlSavedDiagramName.DataSource = lst;
                ddlSavedDiagramName.DataTextField = "name";
                ddlSavedDiagramName.DataValueField = "id";
                ddlSavedDiagramName.DataBind();
            }


        }

        protected void btnAddEntity_Click(object sender, EventArgs e)
        {
            string str = "";
            string ename = txtEntityName.Text;

            if (string.IsNullOrEmpty(txtEntityName.Text))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Entity name cannot be empty.</div>";
                //return;
            }
            else if (entities.Select(en => en.name).ToList().Contains(txtEntityName.Text))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Entity name cannot be duplicated.</div>";
            }
            else if (System.Text.RegularExpressions.Regex.IsMatch(ename, @"^[0-9'@#$%^&*/_]"))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Entity name cannot start with numbers or sympls .</div>";
            }
            else
            {
                EntityModel m = new EntityModel()
                {
                    name = txtEntityName.Text,
                    childAttributes = new List<AttributeModel>(),
                    chilRelationships = new List<RelationshipModel>(),
                    type = "normal"
                };
                entities.Add(m);

                AttributeModel am = new AttributeModel()
                {
                    givenId = m.name + "_id",
                    name = "id",
                    dataType = "int",
                    //length = len,
                    parentEntity = m,
                    //isKey = cbIsPK.Checked ? true : false,
                    attributeType = "normal"
                };

                m.childAttributes.Add(am);
                attributes.Add(am);

                str = "<div class='alert alert-success'><strong>Success!</strong> Entity added successfully.</div>";
            }


            fillEntitiesListDropDown();
            fillCompositeAttributeDropdowns();
            fillRelationshipDropdowns();
            fillRelationshipsListDropdown();

            clearForm();

            drawDiagram();

            alertDiv.InnerHtml = str;

            
        }

        protected void btnEditEntity_Click(object sender, EventArgs e)
        {
            string str = "";

            string ename = txtEditEntityNewName.Text;

            var entity = entities.SingleOrDefault(n => n.name == ddlEditEntitySelect.SelectedValue);

            if (string.IsNullOrEmpty(ename))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Entity name cannot be empty.</div>";
            }
            else if (entities.Select(en => en.name).ToList().Contains(ename))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Entity name cannot be duplicated.</div>";
            }
            else if (System.Text.RegularExpressions.Regex.IsMatch(ename, @"^[0-9'@#$%^&*/_]"))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Entity name cannot start with numbers or sympls .</div>";
            }
            else
            {
                entity.name = ename;

                foreach (var item in entity.childAttributes)
                {
                    foreach (var rel in relationships)
                    {
                        if (rel.primaryAttribute == item.givenId)
                        {
                            rel.primaryAttribute = entity.name + "_" + item.name; ;
                        }
                        if (rel.foriegnAttribute == item.givenId)
                        {
                            rel.foriegnAttribute = entity.name + "_" + item.name; ;
                        }
                    }

                    item.givenId = entity.name + "_" + item.name;
                    str = "<div class='alert alert-success'><strong>Success!</strong> complete Edited successfully.</div>";
                }

            }

            fillEntitiesListDropDown();
            fillRelationshipDropdowns();
            fillCompositeAttributeDropdowns();

            drawDiagram();
            alertDiv.InnerHtml = str;
        }

        protected void ddlEditEntitySelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEditEntitySelect.SelectedIndex != -1)
            {
                var entity = entities.SingleOrDefault(n => n.name == ddlEditEntitySelect.SelectedValue);

                txtEditEntityNewName.Text = entity.name;

            }
        }

        protected void btnDeleteEntity_Click(object sender, EventArgs e)
        {
            string str = "";
            if (entities.Where(c => c.name == ddlDeleteEntity.SelectedItem.Value).ToList().Count > 0)
            {
                EntityModel m = entities.SingleOrDefault(c => c.name == ddlDeleteEntity.SelectedItem.Value);

                entities.Remove(m);

                foreach (var item in attributes.Where(a => a.parentEntity.name == ddlDeleteEntity.SelectedItem.Value))
                {
                    item.status = "Deleted";
                }

                foreach (var item in relationships)
                {
                    var primaryAttribute = attributes.SingleOrDefault(a => a.givenId == item.primaryAttribute);

                    var foreignAttribute = attributes.SingleOrDefault(a => a.givenId == item.foriegnAttribute);

                    if (primaryAttribute.parentEntity.name == ddlDeleteEntity.SelectedItem.Value ||
                        foreignAttribute.parentEntity.name == ddlDeleteEntity.SelectedItem.Value)
                    {
                        item.status = "Deleted";

                    }
                }
            }

            fillEntitiesListDropDown();
            fillRelationshipDropdowns();
            fillCompositeAttributeDropdowns();

            str = "<div class='alert alert-success'><strong>Success!</strong> Entity Deleted successfully.</div>";
            alertDiv.InnerHtml = str;
            drawDiagram();
        }

        protected void btnAddWeakEntity_Click(object sender, EventArgs e)
        {
            alertDiv.InnerHtml = "";

            string str = "";
            string ename = txtWeakEntityName.Text;

            if (string.IsNullOrEmpty(txtWeakEntityName.Text))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Entity name cannot be empty.</div>";
                //return;
            }
            else if (entities.Select(en => en.name).ToList().Contains(txtWeakEntityName.Text))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Entity name cannot be duplicated.</div>";
            }
            else if (System.Text.RegularExpressions.Regex.IsMatch(ename, @"^[0-9'@#$%^&*/_]"))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Entity name cannot start with numbers or sympls .</div>";
            }
            else
            {
                var m = new EntityModel()
                    {
                        name = txtWeakEntityName.Text,
                        childAttributes = new List<AttributeModel>(),
                        chilRelationships = new List<RelationshipModel>(),
                        type = "weak"
                    };

                entities.Add(m);

                AttributeModel am = new AttributeModel()
                {
                    givenId = m.name + "_id",
                    name = "id",
                    dataType = "int",
                    //length = len,
                    parentEntity = m,
                    //isKey = cbIsPK.Checked ? true : false,
                    attributeType = "normal"
                };

                m.childAttributes.Add(am);
                attributes.Add(am);

                str = "<div class='alert alert-success'><strong>Success!</strong> Entity added successfully.</div>";
            }

            fillEntitiesListDropDown();

            drawDiagram();

            alertDiv.InnerHtml = str;
        }

        protected void btnEditWeakEntity_Click(object sender, EventArgs e)
        {
            string str = "";
            var entity = entities.SingleOrDefault(n => n.name == ddlEditWeakEntitySelect.SelectedValue);

            if (!string.IsNullOrEmpty(txtEditWeakEntityNewName.Text))
            {
                entity.name = txtEditWeakEntityNewName.Text;

                foreach (var item in entity.childAttributes)
                {
                    item.givenId = entity.name + "_" + item.name;
                }
            }
            if (entity.type == "weak")
            {
                fillEntitiesListDropDown();

            }

            drawDiagram();
            //fillRelationshipDropdowns();
            str = "<div class='alert alert-success'><strong>Success!</strong> Entity Edited successfully.</div>";
            alertDiv.InnerHtml = str;
        }

        protected void btnDeleteWeakEntity_Click(object sender, EventArgs e)
        {
            string str = "";
            if (entities.Where(c => c.name == ddlDeleteWeakEntity.SelectedItem.Value).ToList().Count > 0)
            {
                EntityModel m = entities.SingleOrDefault(c => c.name == ddlDeleteWeakEntity.SelectedItem.Value);

                entities.Remove(m);

                foreach (var item in attributes.Where(a => a.parentEntity.name == ddlDeleteWeakEntity.SelectedItem.Value))
                {
                    item.status = "Deleted";
                }

                foreach (var item in relationships)
                {
                    var primaryAttribute = attributes.SingleOrDefault(a => a.givenId == item.primaryAttribute);

                    var foreignAttribute = attributes.SingleOrDefault(a => a.givenId == item.foriegnAttribute);

                    if (primaryAttribute.parentEntity.name == ddlDeleteWeakEntity.SelectedItem.Value ||
                        foreignAttribute.parentEntity.name == ddlDeleteWeakEntity.SelectedItem.Value)
                    {
                        item.status = "Deleted";
                    }
                }
            }

            //fillEntitiesListDropDown();
            str = "<div class='alert alert-success'><strong>Success!</strong> Entity Deleted successfully.</div>";
            alertDiv.InnerHtml = str;

        }

        protected void btnAddAtribute_Click(object sender, EventArgs e)
        {
            var en = entities.SingleOrDefault(n => n.name == ddlAttributeEntity.SelectedItem.Text);

            string str = "";
            int len;
            string ename = txtAttributeName.Text;

            if (string.IsNullOrEmpty(txtAttributeName.Text))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Attribute name cannot be empty.</div>";
                //return;
            }

            else if (en.childAttributes.Select(a => a.name).ToList().Contains(txtAttributeName.Text))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Attribute name cannot be duplicated in the same entity.</div>";
            }
            else if (string.IsNullOrEmpty(ddlAttributeDataType.SelectedItem.Value))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Attribute data type must be selected.</div>";
            }
            else if (ddlAttributeDataType.SelectedItem.Value == "Nvarchar" & !int.TryParse(txtAttributeLength.Text, out len))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Attribute length must be number.</div>";
            }
            else if (cbIsPK.Checked == true && en.childAttributes.Where(c => c.isKey == true).ToList().Count > 0)
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Entity cannot have more than one primary key.</div>";
            }
            else if (System.Text.RegularExpressions.Regex.IsMatch(ename, @"^[0-9'@#$%^&*/_]"))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Entity name cannot start with numbers or sympls .</div>";
            }
            else
            {
                AttributeModel am = new AttributeModel()
                {
                    givenId = ddlAttributeEntity.SelectedItem.Text + "_" + txtAttributeName.Text,
                    name = txtAttributeName.Text,
                    dataType = ddlAttributeDataType.SelectedItem.Value,
                    length = len,
                    parentEntity = en,
                    isKey = cbIsPK.Checked ? true : false,
                    attributeType = "normal"
                };

                en.childAttributes.Add(am);
                attributes.Add(am);

                str = "<div class='alert alert-success'><strong>Success!</strong> Attribute added successfully.</div>";
            }
            fillRelationshipDropdowns();

            drawDiagram();




            alertDiv.InnerHtml = str;
        }

        protected void btnEditAttributeSave_Click(object sender, EventArgs e)
        {
            var editAttribute = attributes.SingleOrDefault(a => a.givenId == ddlEditAttributeSelectAttribute.SelectedValue);

            var en = entities.SingleOrDefault(n => n.name == ddlAttributeEntity.SelectedItem.Text);

            string str = "";

            int len;

            if (string.IsNullOrEmpty(txtEditAttributeNewName.Text))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Attribute name cannot be empty.</div>";
                //return;
            }
            else if (en.childAttributes.Select(a => a.name).ToList().Contains(txtEditAttributeNewName.Text))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Attribute name cannot be duplicated in the same entity.</div>";
            }
            else if (ddlEditAttributeNewDataType.SelectedItem.Value == "Nvarchar" & !int.TryParse(txtEditAttributeNewLength.Text, out len))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Attribute length must be number.</div>";
            }
            else
            {
                editAttribute.name = txtEditAttributeNewName.Text;

                editAttribute.givenId = en.name + "_" + editAttribute.name;

                editAttribute.dataType = ddlEditAttributeNewDataType.SelectedValue;

                if (editAttribute.dataType == "Nvarchar")
                {
                    editAttribute.length = len;
                }

                editAttribute.isKey = chkEditAttributeIsPK.Checked;
            }

            fillRelationshipDropdowns();

            drawDiagram();
            str = "<div class='alert alert-success'><strong>Success!</strong> Attribute Edited successfully.</div>";


            alertDiv.InnerHtml = str;
        }

        protected void btnDeleteAttribute_Click(object sender, EventArgs e)
        {
            string str = "";
            if (attributes.Where(c => c.givenId == ddlDeleteAttribute.SelectedItem.Value).ToList().Count > 0)
            {
                AttributeModel m = attributes.SingleOrDefault(c => c.givenId == ddlDeleteAttribute.SelectedItem.Value);

                m.status = "Deleted";

                foreach (var item in relationships)
                {

                    if (item.primaryAttribute == ddlDeleteAttribute.SelectedItem.Value ||
                        item.foriegnAttribute == ddlDeleteAttribute.SelectedItem.Value)
                    {
                        item.status = "Deleted";
                    }
                }
            }
            //fillEntitiesListDropDown();
            str = "<div class='alert alert-success'><strong>Success!</strong> Attribute Deleted successfully.</div>";
            alertDiv.InnerHtml = str;

        }

        protected void btnAddDrivedAtribute_Click(object sender, EventArgs e)
        {
            var en = entities.SingleOrDefault(n => n.name == ddlDrivedAttributeEntity.SelectedItem.Text);

            string str = "";
            int len;
            string ename = txtDrivedAttributeName.Text;

            if (string.IsNullOrEmpty(txtDrivedAttributeName.Text))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Attribute name cannot be empty.</div>";
                //return;
            }
            else if (en.childAttributes.Select(a => a.name).ToList().Contains(txtDrivedAttributeName.Text))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Attribute name cannot be duplicated in the same entity.</div>";
            }
            else if (ddlDrivedAttributeDataType.SelectedItem.Value == "Nvarchar" & !int.TryParse(txtDrivedAttributeLength.Text, out len))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Attribute length must be number.</div>";
            }
            else if (System.Text.RegularExpressions.Regex.IsMatch(ename, @"^[0-9'@#$%^&*/_]"))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Entity name cannot start with numbers or sympls .</div>";
            }
            else
            {
                AttributeModel am = new AttributeModel()
                {
                    givenId = ddlDrivedAttributeEntity.SelectedItem.Text + "_" + txtDrivedAttributeName.Text,
                    name = txtDrivedAttributeName.Text,
                    dataType = ddlDrivedAttributeDataType.SelectedItem.Value,
                    length = len,
                    parentEntity = en,
                    isKey = false,
                    attributeType = "derived"
                };

                en.childAttributes.Add(am);
                attributes.Add(am);

                str = "<div class='alert alert-success'><strong>Success!</strong> Attribute added successfully.</div>";
            }

            drawDiagram();

            fillRelationshipDropdowns();


            alertDiv.InnerHtml = str;
        }

        protected void btnEditDrivedAttributeSave_Click(object sender, EventArgs e)
        {
            var editAttribute = attributes.SingleOrDefault(a => a.givenId == ddlEditDrivedAttributeSelectAttribute.SelectedValue);

            var en = entities.SingleOrDefault(n => n.name == ddlDrivedAttributeEntity.SelectedItem.Text);

            string str = "";

            int len;

            if (string.IsNullOrEmpty(txtEditDrivedAttributeNewName.Text))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Attribute name cannot be empty.</div>";
                //return;
            }
            else if (en.childAttributes.Select(a => a.name).ToList().Contains(txtEditDrivedAttributeNewName.Text))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Attribute name cannot be duplicated in the same entity.</div>";
            }
            else if (ddlEditDrivedAttributeNewDataType.SelectedItem.Value == "Nvarchar" & !int.TryParse(txtEditDrivedAttributeNewLength.Text, out len))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Attribute length must be number.</div>";
            }
            else
            {
                editAttribute.name = txtEditDrivedAttributeNewName.Text;

                editAttribute.givenId = en.name + "_" + editAttribute.name;

                editAttribute.dataType = ddlEditDrivedAttributeNewDataType.SelectedValue;

                if (editAttribute.dataType == "Nvarchar")
                {
                    editAttribute.length = len;
                }
            }

            fillRelationshipDropdowns();

            drawDiagram();
            str = "<div class='alert alert-success'><strong>Success!</strong> Drived Attribute Edited successfully.</div>";
            alertDiv.InnerHtml = str;
        }

        protected void btnDeleteDerivedAttribute_Click(object sender, EventArgs e)
        {
            string str = "";
            if (attributes.Where(c => c.givenId == ddlDeleteDerivedAttribute.SelectedItem.Value).ToList().Count > 0)
            {
                AttributeModel m = attributes.SingleOrDefault(c => c.givenId == ddlDeleteDerivedAttribute.SelectedItem.Value);

                m.status = "Deleted";

                foreach (var item in relationships)
                {

                    if (item.primaryAttribute == ddlDeleteDerivedAttribute.SelectedItem.Value ||
                        item.foriegnAttribute == ddlDeleteDerivedAttribute.SelectedItem.Value)
                    {
                        item.status = "Deleted";
                    }
                }
            }
            str = "<div class='alert alert-success'><strong>Success!</strong> Drived Attribute Deleted successfully.</div>";
            alertDiv.InnerHtml = str;

        }

        protected void btnAddCompositeAtribute_Click(object sender, EventArgs e)
        {
            var en = entities.SingleOrDefault(n => n.name == ddlCompositeAttributeEntity.SelectedItem.Text);

            string str = "";
            //int len;
            string ename = txtCompositeAttributeName.Text;

            if (string.IsNullOrEmpty(txtCompositeAttributeName.Text))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Attribute name cannot be empty.</div>";
                //return;
            }

            else if (en.childAttributes.Where(a => a.status != "Deleted").Select(a => a.name).ToList().Contains(txtCompositeAttributeName.Text))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Attribute name cannot be duplicated in the same entity.</div>";
            }
            else if (System.Text.RegularExpressions.Regex.IsMatch(ename, @"^[0-9'@#$%^&*/_]"))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Entity name cannot start with numbers or sympls .</div>";
            }

            else
            {
                AttributeModel am = new AttributeModel()
                {
                    givenId = ddlCompositeAttributeEntity.SelectedItem.Text + "_" + txtCompositeAttributeName.Text,
                    name = txtCompositeAttributeName.Text,
                    parentEntity = en,
                    isKey = false,
                    attributeType = "composite",
                    compositeChildAttributes = new List<CompositeChildAttributeModel>()
                };

                //CompositeChildAttributeModel cm = new CompositeChildAttributeModel()
                //{
                //    givenId = am.name + "_id",
                //    name = "id",
                //    dataType = "int",
                //    //length = len,
                //    parentAttribute = am,
                //    isKey = true,
                //    attributeType = "normal"
                //};               

                en.childAttributes.Add(am);

                attributes.Add(am);

                fillCompositeAttributeDropdowns();

                str = "<div class='alert alert-success'><strong>Success!</strong> Composite attribute added successfully.</div>";
            }

            drawDiagram();

            fillEntitiesListDropDown();


            alertDiv.InnerHtml = str;
        }

        protected void btnAddCompositeChildAtribute_Click(object sender, EventArgs e)
        {
            var en = attributes.SingleOrDefault(n => n.givenId == ddlChildCompositeAttribute.SelectedItem.Text);

            string str = "";

            int len;

            if (string.IsNullOrEmpty(txtChilAttributeName.Text))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Attribute name cannot be empty.</div>";
                //return;
            }

            else if (en.compositeChildAttributes.Select(a => a.name).ToList().Contains(txtChilAttributeName.Text))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Attribute name cannot be duplicated in the same entity.</div>";
            }
            else if (ddlChildAttributeDataType.SelectedItem.Value == "Nvarchar" & !int.TryParse(txtChildAttributeLength.Text, out len))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Attribute length must be number.</div>";
            }

            else
            {
                CompositeChildAttributeModel am = new CompositeChildAttributeModel()
                {
                    givenId = ddlChildCompositeAttribute.SelectedItem.Text + "_" + txtChilAttributeName.Text,
                    name = txtChilAttributeName.Text,
                    dataType = ddlChildAttributeDataType.SelectedItem.Value,
                    length = len,
                    parentAttribute = en,
                    isKey = false,
                    attributeType = "normal"
                };

                en.compositeChildAttributes.Add(am);
                compositeAttributeChilds.Add(am);

                str = "<div class='alert alert-success'><strong>Success!</strong> Child Attribute added successfully.</div>";
            }
            fillCompositeAttributeDropdowns();
            drawDiagram();

            alertDiv.InnerHtml = str;
        }

        protected void btnEditCompositeAttributeSave_Click(object sender, EventArgs e)
        {
            var editAttribute = attributes.SingleOrDefault(a => a.givenId == ddlEditComositeAttributeSelectAttribute.SelectedValue);

            var en = editAttribute.parentEntity; //entities.SingleOrDefault(n => n.name == ddlCompositeAttributeEntity.SelectedItem.Text);

            string str = "";

            int len;

            if (string.IsNullOrEmpty(txtEditCompositeAttributeNewName.Text))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Attribute name cannot be empty.</div>";
                //return;
            }
            else if (en.childAttributes.Select(a => a.name).ToList().Contains(txtEditCompositeAttributeNewName.Text))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Attribute name cannot be duplicated in the same entity.</div>";
            }
            else if (ddlEditCompositeAttributeNewDataType.SelectedItem.Value == "Nvarchar" & !int.TryParse(txtEditCompositeAttributeNewLength.Text, out len))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Attribute length must be number.</div>";
            }
            else
            {
                editAttribute.name = txtEditCompositeAttributeNewName.Text;

                editAttribute.givenId = en.name + "_" + editAttribute.name;

                editAttribute.dataType = ddlEditCompositeAttributeNewDataType.SelectedValue;

                if (editAttribute.dataType == "Nvarchar")
                {
                    editAttribute.length = len;
                }
            }

            fillCompositeAttributeDropdowns();

            drawDiagram();
            str = "<div class='alert alert-success'><strong>Success!</strong> Composite Attribute Edited successfully.</div>";


            alertDiv.InnerHtml = str;
        }

        protected void btnDeleteCompositeAttribute_Click(object sender, EventArgs e)
        {
            string str = "";

            if (attributes.Where(c => c.givenId == ddlDeleteCompositeAttribute.SelectedItem.Value).ToList().Count > 0)
            {
                AttributeModel m = attributes.SingleOrDefault(c => c.givenId == ddlDeleteCompositeAttribute.SelectedItem.Value);

                m.status = "Deleted";

                foreach (var item in relationships)
                {

                    if (item.primaryAttribute == ddlDeleteCompositeAttribute.SelectedItem.Value ||
                        item.foriegnAttribute == ddlDeleteCompositeAttribute.SelectedItem.Value)
                    {
                        item.status = "Deleted";
                    }
                }
            }

            str = "<div class='alert alert-success'><strong>Success!</strong> Attribute added successfully.</div>";
            alertDiv.InnerHtml = str;

        }

        protected void btnAddMultiValuedAtribute_Click(object sender, EventArgs e)
        {
            var en = entities.SingleOrDefault(n => n.name == ddlMultiValuedAttributeEntity.SelectedValue);

            string str = "";
            int len;// = int.Parse(txtMultiValuedAttributeLength.Text);
            string ename = txtMultiValuedAttributeName.Text;

            if (string.IsNullOrEmpty(txtMultiValuedAttributeName.Text))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Attribute name cannot be empty.</div>";
                //return;
            }
            else if (en.childAttributes.Select(a => a.name).ToList().Contains(txtMultiValuedAttributeName.Text))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Attribute name cannot be duplicated in the same entity.</div>";
            }
            else if (ddlMultiValuedAttributeDataType.SelectedItem.Value == "Nvarchar" & !int.TryParse(txtMultiValuedAttributeLength.Text, out len))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Attribute length must be number.</div>";
            }
            else if (System.Text.RegularExpressions.Regex.IsMatch(ename, @"^[0-9'@#$%^&*/_]"))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Entity name cannot start with numbers or sympls .</div>";
            }
            else
            {
                AttributeModel am = new AttributeModel()
                {
                    givenId = ddlMultiValuedAttributeEntity.SelectedItem.Text + "_" + txtMultiValuedAttributeName.Text,
                    name = txtMultiValuedAttributeName.Text,
                    dataType = ddlMultiValuedAttributeDataType.SelectedItem.Value,
                    length = len,
                    parentEntity = en,
                    isKey = false,
                    attributeType = "multi"
                };

                en.childAttributes.Add(am);
                attributes.Add(am);

                str = "<div class='alert alert-success'><strong>Success!</strong>Multivalued Attribute added successfully.</div>";
            }

            drawDiagram();

            fillRelationshipDropdowns();


            alertDiv.InnerHtml = str;
        }

        protected void btnEditMultiValuedAttributeSave_Click(object sender, EventArgs e)
        {
            var editAttribute = attributes.SingleOrDefault(a => a.givenId == ddlEditMultiValuedAttributeSelectAttribute.SelectedValue);

            var en = entities.SingleOrDefault(n => n.name == ddlMultiValuedAttributeEntity.SelectedItem.Text);

            string str = "";

            int len;

            if (string.IsNullOrEmpty(txtEditMultiValuedAttributeNewName.Text))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Attribute name cannot be empty.</div>";
                //return;
            }
            else if (en.childAttributes.Select(a => a.name).ToList().Contains(txtEditMultiValuedAttributeNewName.Text))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Attribute name cannot be duplicated in the same entity.</div>";
            }
            else if (ddlEditMultiValuedAttributeNewDataType.SelectedItem.Value == "Nvarchar" & !int.TryParse(txtEditMultiValuedAttributeNewLength.Text, out len))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Attribute length must be number.</div>";
            }
            else
            {
                editAttribute.name = txtEditMultiValuedAttributeNewName.Text;

                editAttribute.givenId = en.name + "_" + editAttribute.name;

                editAttribute.dataType = ddlEditMultiValuedAttributeNewDataType.SelectedValue;

                if (editAttribute.dataType == "Nvarchar")
                {
                    editAttribute.length = len;
                }

            }

            fillRelationshipDropdowns();

            drawDiagram();
            str = "<div class='alert alert-success'><strong>Success!</strong>Multivalued Attribute Edited successfully.</div>";

            alertDiv.InnerHtml = str;
        }

        protected void btnDeleteMultiValuedAttribute_Click(object sender, EventArgs e)
        {
            string str = "";
            if (attributes.Where(c => c.givenId == ddlDeleteMultiValuedAttribute.SelectedItem.Value).ToList().Count > 0)
            {
                AttributeModel m = attributes.SingleOrDefault(c => c.givenId == ddlDeleteMultiValuedAttribute.SelectedItem.Value);

                m.status = "Deleted";

                foreach (var item in relationships)
                {

                    if (item.primaryAttribute == ddlDeleteMultiValuedAttribute.SelectedItem.Value ||
                        item.foriegnAttribute == ddlDeleteMultiValuedAttribute.SelectedItem.Value)
                    {
                        item.status = "Deleted";
                    }
                }
            }
            str = "<div class='alert alert-success'><strong>Success!</strong>Multivalued Attribute Deleted successfully.</div>";
            alertDiv.InnerHtml = str;
        }

        protected void btnAddRelationship_Click(object sender, EventArgs e)
        {
            alertDiv.InnerHtml = "";

            string str = "";

            string ename = txtRelationshipName.Text;

            var primaryAttribute = attributes.SingleOrDefault(a => a.givenId == ddlPrimaryAttribute.SelectedValue);

            var forignAttribute = attributes.SingleOrDefault(a => a.givenId == ddlforiegnAttribute.SelectedValue);

            if (string.IsNullOrEmpty(txtRelationshipName.Text))
            {
                //Validation Error
                str = "<div class='alert alert-danger'><strong>Error!</strong> Relationship name cannot be empty.</div>";
            }
            if (relationships.Select(r => r.relationshipName).Contains(txtRelationshipName.Text))
            {
                //Validation Error
                str = "<div class='alert alert-danger'><strong>Error!</strong> Relationship name cannot be duplicated.</div>";
            }
            if (ddlPrimaryAttribute.SelectedValue == null || ddlforiegnAttribute.SelectedValue == null)
            {
                //Validation Error
                str = "<div class='alert alert-danger'><strong>Error!</strong> Primary and Foriegn attribute cannot be empty.</div>";
            }
            if (attributes.SingleOrDefault(a => a.givenId == ddlPrimaryAttribute.SelectedValue).parentEntity.name == attributes.SingleOrDefault(a => a.givenId == ddlforiegnAttribute.SelectedValue).parentEntity.name)
            {
                //Validation Error
                str = "<div class='alert alert-danger'><strong>Error!</strong> Primary and Foriegn tables cannot be the same.</div>";
            }
            else if (System.Text.RegularExpressions.Regex.IsMatch(ename, @"^[0-9'@#$%^&*/_]"))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Entity name cannot start with numbers or sympls .</div>";
            }
            else if (primaryAttribute.dataType.ToLower() != forignAttribute.dataType.ToLower())
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Primary and Foriegn attributes must be the same data type.</div>";
            }

            else
            {
                RelationshipModel r = new RelationshipModel()
                {
                    relationshipName = txtRelationshipName.Text,
                    primaryAttribute = ddlPrimaryAttribute.SelectedValue,
                    foriegnAttribute = ddlforiegnAttribute.SelectedValue,
                    type = ddlRelationshipType.SelectedValue,
                    relationtype = "normal"
                };

                relationships.Add(r);

                var primaryTableName = primaryAttribute.parentEntity.name;

                var primaryTable = entities.SingleOrDefault(n => n.name == primaryTableName);

                primaryTable.chilRelationships.Add(r);

                str = "<div class='alert alert-success'><strong>Success!</strong> Relationship added successfully.</div>";
            }

            fillRelationshipsListDropdown();

            drawDiagram();

            alertDiv.InnerHtml = str;
        }

        protected void btnEditRelationship_Click(object sender, EventArgs e)
        {
            alertDiv.InnerHtml = "";

            string str = "";

            var editRelationship = relationships.SingleOrDefault(n => n.relationshipName == ddlEditRelationshipSelect.SelectedValue);

            if (string.IsNullOrEmpty(txtEditRelationshipNewName.Text))
            {
                //Validation Error
                str = "<div class='alert alert-danger'><strong>Error!</strong> Relationship name cannot be empty.</div>";
            }
            if (relationships.Select(r => r.relationshipName).Contains(txtEditRelationshipNewName.Text))
            {
                //Validation Error
                str = "<div class='alert alert-danger'><strong>Error!</strong> Relationship name cannot be duplicated.</div>";
            }
            if (ddlEditPrimaryAttribute.SelectedValue == null || ddlEditforiegnAttribute.SelectedValue == null)
            {
                //Validation Error
                str = "<div class='alert alert-danger'><strong>Error!</strong> Primary and Foriegn attribute cannot be empty.</div>";
            }
            if (attributes.SingleOrDefault(a => a.givenId == ddlEditPrimaryAttribute.SelectedValue).parentEntity.name == attributes.SingleOrDefault(a => a.givenId == ddlEditforiegnAttribute.SelectedValue).parentEntity.name)
            {
                //Validation Error
                str = "<div class='alert alert-danger'><strong>Error!</strong> Primary and Foriegn tables cannot be the same.</div>";
            }
            else
            {
                editRelationship.relationshipName = txtEditRelationshipNewName.Text;
                editRelationship.primaryAttribute = ddlEditPrimaryAttribute.SelectedValue;
                editRelationship.foriegnAttribute = ddlEditforiegnAttribute.SelectedValue;
                editRelationship.type = ddlEditRelationshipType.SelectedValue;
            }


            fillRelationshipDropdowns();
            fillRelationshipsListDropdown();

            drawDiagram();
            str = "<div class='alert alert-success'><strong>Success!</strong> Relationship Edited successfully.</div>";
            alertDiv.InnerHtml = str;
        }

        protected void btnAddWeakRelationship_Click(object sender, EventArgs e)
        {
            alertDiv.InnerHtml = "";

            string str = "";
            string ename = txtWeakEntityName.Text;

            if (string.IsNullOrEmpty(txtWeakRelationshipName.Text))
            {
                //Validation Error
                str = "<div class='alert alert-danger'><strong>Error!</strong> Relationship name cannot be empty.</div>";
            }
            if (relationships.Select(r => r.relationshipName).Contains(txtWeakRelationshipName.Text))
            {
                //Validation Error
                str = "<div class='alert alert-danger'><strong>Error!</strong> Relationship name cannot be duplicated.</div>";
            }
            if (ddlWeakPrimaryAttribute.SelectedValue == null || ddlWeakforiegnAttribute.SelectedValue == null)
            {
                //Validation Error
                str = "<div class='alert alert-danger'><strong>Error!</strong> Primary and Foriegn attribute cannot be empty.</div>";
            }
            if (attributes.SingleOrDefault(a => a.givenId == ddlWeakPrimaryAttribute.SelectedValue).parentEntity.name == attributes.SingleOrDefault(a => a.givenId == ddlWeakforiegnAttribute.SelectedValue).parentEntity.name)
            {
                //Validation Erro
                str = "<div class='alert alert-danger'><strong>Error!</strong> Primary and Foriegn tables cannot be the same.</div>";
            }
            else if (System.Text.RegularExpressions.Regex.IsMatch(ename, @"^[0-9'@#$%^&*/_]"))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Entity name cannot start with numbers or sympls .</div>";
            }
            else
            {
                RelationshipModel r = new RelationshipModel()
                {
                    relationshipName = txtWeakRelationshipName.Text,
                    primaryAttribute = ddlWeakPrimaryAttribute.SelectedValue,
                    foriegnAttribute = ddlWeakforiegnAttribute.SelectedValue,
                    type = ddlWeakRelationshipType.SelectedValue,
                    relationtype = "weak"
                };

                relationships.Add(r);

                var primaryAttribute = attributes.SingleOrDefault(a => a.givenId == r.primaryAttribute);

                var primaryTableName = primaryAttribute.parentEntity.name;

                var primaryTable = entities.SingleOrDefault(n => n.name == primaryTableName);

                primaryTable.chilRelationships.Add(r);

                str = "<div class='alert alert-success'><strong>Success!</strong> Weak Relationship added successfully.</div>";
            }

            fillRelationshipsListDropdown();


            drawDiagram();


            alertDiv.InnerHtml = str;
        }

        protected void btnEditWeakRelationship_Click(object sender, EventArgs e)
        {
            alertDiv.InnerHtml = "";

            string str = "";

            var editRelationship = relationships.SingleOrDefault(n => n.relationshipName == ddlEditWeakRelationshipSelect.SelectedValue);

            if (string.IsNullOrEmpty(txtEditWeakRelationshipNewName.Text))
            {
                //Validation Error
                str = "<div class='alert alert-danger'><strong>Error!</strong> Relationship name cannot be empty.</div>";
            }
            else if (relationships.Select(r => r.relationshipName).Contains(txtEditWeakRelationshipNewName.Text))
            {
                //Validation Error
                str = "<div class='alert alert-danger'><strong>Error!</strong> Relationship name cannot be duplicated.</div>";
            }
            else if (ddlEditWeakPrimaryAttribute.SelectedValue == null || ddlEditWeakforiegnAttribute.SelectedValue == null)
            {
                //Validation Error
                str = "<div class='alert alert-danger'><strong>Error!</strong> Primary and Foriegn attribute cannot be empty.</div>";
            }
            else if (attributes.SingleOrDefault(a => a.givenId == ddlEditWeakPrimaryAttribute.SelectedValue).parentEntity.name == attributes.SingleOrDefault(a => a.givenId == ddlEditWeakforiegnAttribute.SelectedValue).parentEntity.name)
            {
                //Validation Error
                str = "<div class='alert alert-danger'><strong>Error!</strong> Primary and Foriegn tables cannot be the same.</div>";
            }
            else
            {
                editRelationship.relationshipName = txtEditWeakRelationshipNewName.Text;
                editRelationship.primaryAttribute = ddlEditWeakPrimaryAttribute.SelectedValue;
                editRelationship.foriegnAttribute = ddlEditWeakforiegnAttribute.SelectedValue;
                editRelationship.type = ddlEditWeakRelationshipType.SelectedValue;
            }

            fillRelationshipDropdowns();

            fillRelationshipsListDropdown();

            drawDiagram();
            str = "<div class='alert alert-success'><strong>Success!</strong>Weak Relationship Edied successfully.</div>";

            alertDiv.InnerHtml = str;
        }

        public void fillEntitiesListDropDown()
        {
            if (entities.Count > 0)
            {
                ddlAttributeEntity.DataSource = entities;
                ddlAttributeEntity.DataTextField = "name";
                ddlAttributeEntity.DataValueField = "name";
                ddlAttributeEntity.DataBind();


                ddlEditEntitySelect.DataSource = entities;
                ddlEditEntitySelect.DataTextField = "name";
                ddlEditEntitySelect.DataValueField = "name";
                ddlEditEntitySelect.DataBind();

                ddlEditWeakEntitySelect.DataSource = entities;
                ddlEditWeakEntitySelect.DataTextField = "name";
                ddlEditWeakEntitySelect.DataValueField = "name";
                ddlEditWeakEntitySelect.DataBind();

                ddlDrivedAttributeEntity.DataSource = entities;
                ddlDrivedAttributeEntity.DataTextField = "name";
                ddlDrivedAttributeEntity.DataValueField = "name";
                ddlDrivedAttributeEntity.DataBind();

                ddlMultiValuedAttributeEntity.DataSource = entities;
                ddlMultiValuedAttributeEntity.DataTextField = "name";
                ddlMultiValuedAttributeEntity.DataValueField = "name";
                ddlMultiValuedAttributeEntity.DataBind();

                ddlCompositeAttributeEntity.DataSource = entities;
                ddlCompositeAttributeEntity.DataTextField = "name";
                ddlCompositeAttributeEntity.DataValueField = "name";
                ddlCompositeAttributeEntity.DataBind();

                ddlDeleteEntity.DataSource = entities;
                ddlDeleteEntity.DataTextField = "name";
                ddlDeleteEntity.DataValueField = "name";
                ddlDeleteEntity.DataBind();

                ddlDeleteWeakEntity.DataSource = entities;
                ddlDeleteWeakEntity.DataTextField = "name";
                ddlDeleteWeakEntity.DataValueField = "name";
                ddlDeleteWeakEntity.DataBind();


            }
        }

        public void fillRelationshipDropdowns()
        {
            var lst = attributes.Where(a => a.attributeType == "normal");

            if (attributes.Count > 0)
            {
                ddlPrimaryAttribute.DataSource = lst;
                ddlPrimaryAttribute.DataTextField = "givenid";
                ddlPrimaryAttribute.DataValueField = "givenid";
                ddlPrimaryAttribute.DataBind();

                ddlforiegnAttribute.DataSource = lst;
                ddlforiegnAttribute.DataTextField = "givenid";
                ddlforiegnAttribute.DataValueField = "givenid";
                ddlforiegnAttribute.DataBind();

                //dropdown list of primery/foriegn for weak relationship
                ddlWeakPrimaryAttribute.DataSource = lst;
                ddlWeakPrimaryAttribute.DataTextField = "givenid";
                ddlWeakPrimaryAttribute.DataValueField = "givenid";
                ddlWeakPrimaryAttribute.DataBind();

                ddlWeakforiegnAttribute.DataSource = lst;
                ddlWeakforiegnAttribute.DataTextField = "givenid";
                ddlWeakforiegnAttribute.DataValueField = "givenid";
                ddlWeakforiegnAttribute.DataBind();

                ddlEditAttributeSelectAttribute.DataSource = lst;
                ddlEditAttributeSelectAttribute.DataTextField = "givenid";
                ddlEditAttributeSelectAttribute.DataValueField = "givenid";
                ddlEditAttributeSelectAttribute.DataBind();

                ddlEditPrimaryAttribute.DataSource = lst;
                ddlEditPrimaryAttribute.DataTextField = "givenid";
                ddlEditPrimaryAttribute.DataValueField = "givenid";
                ddlEditPrimaryAttribute.DataBind();

                ddlEditforiegnAttribute.DataSource = lst;
                ddlEditforiegnAttribute.DataTextField = "givenid";
                ddlEditforiegnAttribute.DataValueField = "givenid";
                ddlEditforiegnAttribute.DataBind();

                //fill dropdown list for derived attribute
                ddlEditDrivedAttributeSelectAttribute.DataSource = lst;
                ddlEditDrivedAttributeSelectAttribute.DataTextField = "givenid";
                ddlEditDrivedAttributeSelectAttribute.DataValueField = "givenid";
                ddlEditDrivedAttributeSelectAttribute.DataBind();

                //fill dropdown list for Multivalued attribute
                ddlEditMultiValuedAttributeSelectAttribute.DataSource = lst;
                ddlEditMultiValuedAttributeSelectAttribute.DataTextField = "givenid";
                ddlEditMultiValuedAttributeSelectAttribute.DataValueField = "givenid";
                ddlEditMultiValuedAttributeSelectAttribute.DataBind();


                //delete attribute
                ddlDeleteAttribute.DataSource = lst;
                ddlDeleteAttribute.DataTextField = "givenid";
                ddlDeleteAttribute.DataValueField = "givenid";
                ddlDeleteAttribute.DataBind();

                ddlDeleteDerivedAttribute.DataSource = lst;
                ddlDeleteDerivedAttribute.DataTextField = "givenid";
                ddlDeleteDerivedAttribute.DataValueField = "givenid";
                ddlDeleteDerivedAttribute.DataBind();


                ddlDeleteMultiValuedAttribute.DataSource = lst;
                ddlDeleteMultiValuedAttribute.DataTextField = "givenid";
                ddlDeleteMultiValuedAttribute.DataValueField = "givenid";
                ddlDeleteMultiValuedAttribute.DataBind();


                ddlEditWeakPrimaryAttribute.DataSource = lst;
                ddlEditWeakPrimaryAttribute.DataTextField = "givenid";
                ddlEditWeakPrimaryAttribute.DataValueField = "givenid";
                ddlEditWeakPrimaryAttribute.DataBind();


                ddlEditWeakforiegnAttribute.DataSource = lst;
                ddlEditWeakforiegnAttribute.DataTextField = "givenid";
                ddlEditWeakforiegnAttribute.DataValueField = "givenid";
                ddlEditWeakforiegnAttribute.DataBind();


                ddlEditPrimaryAttribute.DataSource = lst;
                ddlEditPrimaryAttribute.DataTextField = "givenid";
                ddlEditPrimaryAttribute.DataValueField = "givenid";
                ddlEditPrimaryAttribute.DataBind();


                ddlEditforiegnAttribute.DataSource = lst;
                ddlEditforiegnAttribute.DataTextField = "givenid";
                ddlEditforiegnAttribute.DataValueField = "givenid";
                ddlEditforiegnAttribute.DataBind();
            }
        }

        public void fillRelationshipsListDropdown()
        {
            var normallst = relationships.Where(c => c.status != "Deleted" && c.relationtype == "normal").ToList();
            var weaklst = relationships.Where(c => c.status != "Deleted" && c.relationtype == "weak").ToList();

            ddlEditRelationshipSelect.DataSource = normallst;
            ddlEditRelationshipSelect.DataTextField = "relationshipName";
            ddlEditRelationshipSelect.DataValueField = "relationshipName";
            ddlEditRelationshipSelect.DataBind();

            ddlEditWeakRelationshipSelect.DataSource = weaklst;
            ddlEditWeakRelationshipSelect.DataTextField = "relationshipName";
            ddlEditWeakRelationshipSelect.DataValueField = "relationshipName";
            ddlEditWeakRelationshipSelect.DataBind();
        }
        public void fillCompositeAttributeDropdowns()
        {
            var lst = attributes.Where(c => c.attributeType == "composite").ToList();

            if (lst.Count > 0)
            {
                ddlChildCompositeAttribute.DataSource = lst;
                ddlChildCompositeAttribute.DataTextField = "givenid";
                ddlChildCompositeAttribute.DataValueField = "givenid";
                ddlChildCompositeAttribute.DataBind();


                ddlDeleteCompositeAttribute.DataSource = lst;
                ddlDeleteCompositeAttribute.DataTextField = "givenid";
                ddlDeleteCompositeAttribute.DataValueField = "givenid";
                ddlDeleteCompositeAttribute.DataBind();

            }
        }

        protected void btnGenerateDatabase_Click(object sender, EventArgs e)
        {
            alertDiv.InnerHtml = "";

            string str = "";

            if (string.IsNullOrEmpty(txtHostName.Text))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Hostname cannot be null!</div>";

            }

            try
            {
                var connectionString = @"Data Source=" + txtHostName.Text + ";User ID=" + txtUsername.Text + ";Password=" + txtPassword.Text;

                string script = "CREATE DATABASE " + txtDatabaseName.Text;

                SqlConnection con = new SqlConnection(connectionString);

                SqlCommand cmd = new SqlCommand(script, con);

                con.Open();

                cmd.ExecuteNonQuery();

                con.Close();

                script = "USE " + txtDatabaseName.Text;

                script += ";";

                foreach (var item in entities)
                {
                    script += " CREATE TABLE [" + item.name + "](";

                    int counter = 0;

                    int max = item.childAttributes.Where(a => a.attributeType != "multi").Count();

                    foreach (var a in item.childAttributes.Where(c => c.attributeType != "composite").ToList())
                    {
                        if (a.attributeType != "multi")
                        {
                            script += a.name + " " + a.dataType + "";

                            if (a.dataType == "Nvarchar")
                            {
                                script += "(" + a.length + ")";
                            }
                            if (a.name == "id")
                            {
                                if (item.childAttributes.Where(c => c.isKey == true).ToList().Count == 0)
                                {
                                    script += " PRIMARY KEY IDENTITY(1,1) NOT NULL ";
                                }
                                else
                                {
                                    script += " IDENTITY(1,1) UNIQUE NOT NULL ";
                                }
                            }
                            if (a.isKey)
                            {
                                script += " PRIMARY KEY NOT NULL";
                            }

                            counter++;

                            if (counter != max)
                            {
                                script += ",";
                            }
                        }

                    }


                    script += ")";

                    script += " ;";


                    foreach (var a in item.childAttributes)
                    {
                        if (a.attributeType == "multi")
                        {
                            script += " CREATE TABLE [" + a.givenId + "](Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL, " + a.name + " " + a.dataType;

                            if (a.dataType == "Nvarchar")
                            {
                                script += "(" + a.length + ") ";
                            }

                            script += ", " + item.name + "Id INT);";

                            script += "ALTER TABLE " + a.givenId + " ADD FOREIGN KEY (" + item.name + "Id) REFERENCES " + item.name + "(Id); ";
                        }
                        if (a.attributeType == "composite")
                        {
                            script += " CREATE TABLE [" + a.givenId + "](Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL ";

                            foreach (var ca in a.compositeChildAttributes)
                            {
                                script += ", " + ca.name + " " + ca.dataType;
                                if (ca.dataType == "Nvarchar")
                                {
                                    script += "(" + ca.length + ") ";
                                }
                            }

                            script += ", " + item.name + "Id INT);";

                            script += "ALTER TABLE " + a.givenId + " ADD FOREIGN KEY (" + item.name + "Id) REFERENCES " + item.name + "(Id); ";
                        }
                    }
                }

                foreach (var item in relationships)
                {
                    var fromAttribute = attributes.SingleOrDefault(a => a.givenId == item.primaryAttribute);
                    var toAttribute = attributes.SingleOrDefault(a => a.givenId == item.foriegnAttribute);
                    var fromTable = fromAttribute.parentEntity.name;
                    var toTable = toAttribute.parentEntity.name;

                    script += "ALTER TABLE [" + fromTable + "] ADD FOREIGN KEY (" + fromAttribute.name + ") REFERENCES [" + toTable + "](" + toAttribute.name + "); ";


                }

                cmd = new SqlCommand(script, con);

                con.Open();

                cmd.ExecuteNonQuery();

                con.Close();

                str = "<div class='alert alert-success'><strong>Success!</strong> Database generated successfully.</div>";

            }
            catch (Exception ex)
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> " + ex.Message + ".</div>";
                throw ex;
            }
            drawDiagram();

            alertDiv.InnerHtml = str;

        }

        protected void btnSaveDiagram_Click(object sender, EventArgs e)
        {
            alertDiv.InnerHtml = "";

            string str = "";

            var lst = listSavedDiagrams();

            if (string.IsNullOrEmpty(txtDiagramName.Text.Trim()))
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Diagram name cannot be empty.</div>";
                //return;
            }

            var diagramName = txtDiagramName.Text.Trim();

            //if (lst.Select(d => d.name).Contains(diagramName))
            //{
            //    str = "<div class='alert alert-danger'><strong>Error!</strong> Diagram name already exists. Please choose another name.</div>";
            //    //return;
            //}

            if (entities.Count == 0)
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Diagram is empty. You cannot save empty diagram</div>";
                //return;
            }

            try
            {
                var cmdstr = "";

                bool result = true;

                if (lst.Select(d => d.name).Contains(diagramName))
                {
                    cmdstr = "DELETE FROM SavedDiagram WHERE Name = '" + diagramName + "'";

                    result = saveToDb(cstr, cmdstr);
                }

                cmdstr = "INSERT INTO SavedDiagram(Name) VALUES('" + diagramName + "')";

                result = saveToDb(cstr, cmdstr);


                if (result)
                {
                    lst = listSavedDiagrams();

                    var diagram = lst.SingleOrDefault(d => d.name == diagramName);

                    diagram.entities = listSavedEntities(diagram.id);

                    foreach (var item in entities.Where(c => c.status != "Deleted").ToList())
                    {
                        if (!diagram.entities.Select(d => d.name).Contains(item.name))
                        {
                            cmdstr = "INSERT INTO tblEntity(name, type, diagramid) VALUES('" + item.name + "','" + item.type + "'," + diagram.id + ")";

                            result = saveToDb(cstr, cmdstr);
                        }

                        if (result)
                        {
                            var elst = listSavedEntities(diagram.id);
                            var ent = elst.SingleOrDefault(en => en.name == item.name);
                            ent.childAttributes = listSavedAttriutes(ent.id);

                            foreach (var item2 in attributes.Where(a => a.parentEntity.name == ent.name && a.status != "Deleted"))
                            {
                                //if (!ent.childAttributes.Select(a => a.name).Contains(item2.name))
                                //{
                                int ik = item2.isKey ? 1 : 0;

                                if (item2.attributeType == "composite")
                                {
                                    cmdstr = "INSERT INTO tblAttribute(name, givenid, parententityid, iskey, attributetype) VALUES('" +
                                    item2.name + "','" + item2.givenId + "'," + ent.id + ","
                                    + ik + ",'composite')";
                                }
                                else
                                {
                                    cmdstr = "INSERT INTO tblAttribute(name, givenid, parententityid, datatype, length, iskey, attributetype) VALUES('" +
                                    item2.name + "','" + item2.givenId + "'," + ent.id + ",'"
                                    + item2.dataType + "'," + item2.length + "," + ik + ",'" + item2.attributeType + "')";
                                }

                                saveToDb(cstr, cmdstr);
                                //}

                            }

                            var attlst = listSavedAttriutes(ent.id).Where(c => c.attributeType == "composite").ToList();

                            foreach (var attr in attlst)
                            {
                                attr.compositeChildAttributes = compositeAttributeChilds.Where(c => c.parentAttribute.givenId == attr.givenId).ToList();

                                foreach (var ca in attr.compositeChildAttributes)
                                {
                                    int ik = attr.isKey ? 1 : 0;

                                    cmdstr = "INSERT INTO tblCompositeChildAttribute(name, givenid, parentattributeid, datatype, length, iskey) VALUES('" +
                                    ca.name + "','" + ca.givenId + "'," + attr.id + ",'"
                                    + ca.dataType + "'," + ca.length + "," + ik + ")";

                                    saveToDb(cstr, cmdstr);
                                }
                            }

                        }
                    }

                    foreach (var item in entities.Where(c => c.status != "Deleted").ToList())
                    {
                        var la = new List<AttributeModel>();
                        var sa = listSavedEntities(diagram.id);

                        foreach (var r in sa)
                        {
                            r.childAttributes = listSavedAttriutes(r.id);
                            foreach (var ea in r.childAttributes)
                            {
                                la.Add(ea);
                            }
                        }

                        foreach (var rel in item.chilRelationships.Where(c => c.status != "Deleted"))
                        {
                            var foriegnAttribute = la.SingleOrDefault(a => a.givenId == rel.foriegnAttribute);
                            var primaryAttribute = la.SingleOrDefault(a => a.givenId == rel.primaryAttribute);

                            cmdstr = "INSERT INTO tblRelationship([Name],[PKId],[FKId],[type],[diagramId],[relType]) VALUES('" +
                                txtRelationshipName.Text + "','" + primaryAttribute.id + "','" + foriegnAttribute.id + "','" + ddlRelationshipType.SelectedValue + "'," + diagram.id + ",'" + rel.relationtype + "'" + ")";

                            saveToDb(cstr, cmdstr);
                        }
                    }

                    str = "<div class='alert alert-success'><strong>Success!</strong> Diagram saved successfully.</div>";
                }
                else
                {
                    str = "<div class='alert alert-danger'><strong>Error!</strong> Saving diagram to database failed. Please check database connectivity</div>";
                }

            }
            catch (Exception ex)
            {
                str = "<div class='alert alert-danger'><strong>Error!</strong> Saving diagram to database failed. Please check database connectivity</div>";
                throw ex;
            }

            drawDiagram();

            alertDiv.InnerHtml = str;
        }

        protected void btnOpenDiagram_Click(object sender, EventArgs e)
        {
            try
            {
                entities = new List<EntityModel>();

                attributes = new List<AttributeModel>();

                relationships = new List<RelationshipModel>();

                compositeAttributeChilds = new List<CompositeChildAttributeModel>();

                var did = int.Parse(ddlSavedDiagramName.SelectedValue);

                txtDiagramName.Text = ddlSavedDiagramName.SelectedItem.Text;

                entities = listSavedEntities(did);

                foreach (var item in entities)
                {
                    item.childAttributes = listSavedAttriutes(item.id);

                    foreach (var ea in item.childAttributes)
                    {
                        ea.parentEntity = item;

                        attributes.Add(ea);

                        if (ea.attributeType == "composite")
                        {
                            ea.compositeChildAttributes = listSavedCompositeChildAttriutes(ea.id);

                            foreach (var ca in ea.compositeChildAttributes)
                            {
                                ca.parentAttribute = ea;
                                compositeAttributeChilds.Add(ca);
                            }
                        }
                    }

                }

                relationships = listSavedRelationships(did);

                foreach (var item in relationships)
                {
                    var primaryAttribute = attributes.SingleOrDefault(a => a.givenId == item.primaryAttribute);

                    var primaryTableName = primaryAttribute.parentEntity.name;

                    var primaryTable = entities.SingleOrDefault(n => n.name == primaryTableName);

                    primaryTable.chilRelationships.Add(item);
                }


                fillEntitiesListDropDown();

                fillRelationshipDropdowns();

                fillRelationshipsListDropdown();

                fillCompositeAttributeDropdowns();

                drawDiagram();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool saveToDb(string constr, string cmdStr)
        {
            var con = new SqlConnection(constr);

            try
            {
                var cmd = new SqlCommand(cmdStr, con);

                con.Open();

                cmd.ExecuteNonQuery();

                con.Close();

                return true;
            }
            catch (Exception ex)
            {
                //throw;
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();

                //return false;
                throw ex;
            }
        }

        public List<DiagramModel> listSavedDiagrams()
        {
            //var constr = "";

            var con = new SqlConnection(cstr);

            var lst = new List<DiagramModel>();

            try
            {
                var cmd = new SqlCommand("SELECT * FROM SavedDiagram", con);

                con.Open();

                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lst.Add(new DiagramModel()
                    {
                        id = int.Parse(dr["id"].ToString()),
                        name = dr["name"].ToString()
                    });
                }

                con.Close();

                return lst;
            }
            catch (Exception)
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();

                return null;
                //throw;
            }
        }

        public List<EntityModel> listSavedEntities(int diagramId)
        {
            //var constr = "";

            var con = new SqlConnection(cstr);

            var lst = new List<EntityModel>();

            try
            {
                var cmd = new SqlCommand("SELECT * FROM tblEntity WHERE diagramId=" + diagramId, con);

                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();

                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lst.Add(new EntityModel()
                    {
                        id = int.Parse(dr["id"].ToString()),
                        name = dr["name"].ToString(),
                        type = dr["type"].ToString(),
                        childAttributes = listSavedAttriutes(int.Parse(dr["id"].ToString())),
                        chilRelationships = new List<RelationshipModel>()
                    });
                }

                con.Close();

                return lst;
            }
            catch (Exception)
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();

                return null;
                //throw;
            }
        }

        public List<AttributeModel> listSavedAttriutes(int entityId)
        {
            //var constr = "";

            var con = new SqlConnection(cstr);

            var lst = new List<AttributeModel>();

            try
            {
                var cmd = new SqlCommand("SELECT * FROM tblAttribute WHERE ParentEntityId=" + entityId, con);

                con.Open();

                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (dr["attributetype"].ToString() == "composite")
                    {
                        lst.Add(new AttributeModel()
                        {
                            id = int.Parse(dr["id"].ToString()),
                            name = dr["name"].ToString(),
                            givenId = dr["givenId"].ToString(),
                            isKey = bool.Parse(dr["iskey"].ToString()),
                            attributeType = dr["attributetype"].ToString()
                        });
                    }
                    else
                    {
                        lst.Add(new AttributeModel()
                        {
                            id = int.Parse(dr["id"].ToString()),
                            name = dr["name"].ToString(),
                            givenId = dr["givenId"].ToString(),
                            dataType = dr["DataType"].ToString(),
                            length = int.Parse(dr["length"].ToString()),
                            isKey = bool.Parse(dr["iskey"].ToString()),
                            attributeType = dr["attributetype"].ToString()
                        });
                    }

                }

                con.Close();

                return lst;
            }
            catch (Exception)
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();

                return null;
                //throw;
            }
        }

        public List<CompositeChildAttributeModel> listSavedCompositeChildAttriutes(int attributeId)
        {
            //var constr = "";

            var con = new SqlConnection(cstr);

            var lst = new List<CompositeChildAttributeModel>();

            try
            {
                var cmd = new SqlCommand("SELECT * FROM tblCompositeChildAttribute WHERE ParentAttributeId=" + attributeId, con);

                con.Open();

                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lst.Add(new CompositeChildAttributeModel()
                    {
                        id = int.Parse(dr["id"].ToString()),
                        name = dr["name"].ToString(),
                        givenId = dr["givenId"].ToString(),
                        dataType = dr["DataType"].ToString(),
                        length = int.Parse(dr["length"].ToString()),
                        isKey = bool.Parse(dr["iskey"].ToString())
                    });
                }

                con.Close();

                return lst;
            }
            catch (Exception)
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();

                return null;
                //throw;
            }
        }

        public List<RelationshipModel> listSavedRelationships(int diagramId)
        {
            //var constr = "";

            var con = new SqlConnection(cstr);

            var lst = new List<RelationshipModel>();

            try
            {
                var cmd = new SqlCommand("SELECT r.Id,r.Name,r.PKId, pa.GivenId as PKGivenName,r.FKId, fa.GivenId as FKGivenName, r.type,r.diagramId, r.reltype FROM tblRelationship r JOIN tblAttribute pa ON r.PKId = pa.id JOIN tblAttribute fa ON r.FKId = fa.Id WHERE diagramId = " + diagramId, con);

                con.Open();

                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lst.Add(new RelationshipModel()
                    {
                        id = int.Parse(dr["id"].ToString()),
                        relationshipName = dr["name"].ToString(),
                        primaryAttribute = dr["PKGivenName"].ToString(),
                        foriegnAttribute = dr["FKGivenName"].ToString(),
                        type = dr["type"].ToString(),
                        diagramId = int.Parse(dr["diagramId"].ToString()),
                        relationtype = dr["reltype"].ToString()
                    });
                }

                con.Close();

                return lst;
            }
            catch (Exception)
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();

                return null;
                //throw;
            }
        }

        public void drawDiagram()
        {
            int columnsCount = 0;
            rightDiv.InnerHtml = "";
            string str = "<table><tr>";
            foreach (var item in entities.Where(c => c.status != "Deleted").ToList())
            {

                if (columnsCount == 4)
                {
                    str += "</tr><tr>";
                    columnsCount = 0;
                }

                str += "<td>";
                foreach (var attr in item.childAttributes.Where(c => c.status != "Deleted").ToList())
                {
                    if (attr.attributeType == "normal")
                    {
                        str += "<div class='drag' id='" + item.name + "_" + attr.name + "' style='min-width: 70px; min-height: 30px;background: #00B95C;-moz-border-radius: 70px /30px;-webkit-border-radius: 70px / 30px;border-radius: 70px / 30px;float: left;margin: 5px;margin-bottom: 25px;text-align: center;vertical-align: middle;line-height: 25px;'>";
                    }
                    else if (attr.attributeType == "derived")
                    {
                        str += "<div class='drag' id='" + item.name + "_" + attr.name + "' style='min-width: 70px; min-height: 30px;  -moz-border-radius: 70px / 30px;-webkit-border-radius: 70px / 30px;border-radius: 70px / 30px; background-color: #FFFFFF; border: 2px dotted #000000; -moz-border: 2px dotted #000000; text-align: center;float:left; margin: 5px; margin-bottom: 25px; vertical-align: middle; line-height: 25px;'>";
                    }
                    else if (attr.attributeType == "multi")
                    {
                        str += "<div class='drag' id='" + item.name + "_" + attr.name + "' style='min-width: 70px; flex: auto;min-height: 30px;  -moz-border-radius: 70px / 30px;-webkit-border-radius: 70px / 30px;border-radius: 70px / 30px; background-color: #99D9EA; border: 2px double #000000; -moz-border: 2px double #000000; text-align: center;float:left; margin: 5px; margin-bottom: 25px; vertical-align: middle; line-height: 25px; border-top: 5px double ;border-right: 5px double ;border-bottom: 5px double ;border-left: 5px double ;'>";
                    }
                    else if (attr.attributeType == "composite")
                    {
                        str += "<table><tr>";

                        foreach (var ca in attr.compositeChildAttributes.Where(c => c.status != "Deleted").ToList())
                        {
                            str += "<td><div class='drag' id='" + ca.givenId + "' style='min-width: 70px; height: 30px; background: #00B95C;-moz-border-radius: 70px /30px;-webkit-border-radius: 70px / 30px;border-radius: 70px / 30px;float: left;margin: 5px;margin-bottom: 25px;text-align: center;vertical-align: middle;line-height: 25px;'>" + ca.name + "</div></td>";
                        }

                        str += "</tr></table>";

                        str += "<div class='drag' id='" + item.name + "_" + attr.name + "' style='min-width: 70px; min-height: 30px; background: #f9f902;-moz-border-radius: 70px /30px;-webkit-border-radius: 70px / 30px;border-radius: 70px / 30px;float: left;margin: 5px;margin-bottom: 25px;text-align: center;vertical-align: middle;line-height: 25px;'>";
                    }

                    if (attr.isKey)
                    {
                        str += "<u><b>";
                    }

                    str += attr.name;

                    if (attr.isKey)
                    {
                        str += "</b></u>";
                    }


                    str += "</div>";
                }

                str += "<br/>";
                if (item.type == "normal")
                {
                    str += "<div class='drag' id='" + item.name + "' style='width: 90px; height: 50px; flex-flow: column;background: #4B55CF;margin: 20px;text-align: center;'><b>";
                }
                else if (item.type == "weak")
                {
                    str += "<div class='drag'  id='" + item.name + "' style='width: 90px; height: 50px; background: #FF9E5E;margin: 20px;text-align: center;vertical-align: middle;line-height: 40px;border-style: double; border-top: 5px double ;border-right: 5px double ;border-bottom: 5px double ;border-left: 5px double ;'><b>";

                }
                str += item.name;

                str += "</b></div></td>";

                columnsCount++;

                foreach (var r in item.chilRelationships.Where(c => c.status != "Deleted").ToList())
                {
                    if (r.relationtype == "normal")
                    {
                        var fromAttribute = attributes.SingleOrDefault(a => a.givenId == r.primaryAttribute);
                        var toAttribute = attributes.SingleOrDefault(a => a.givenId == r.foriegnAttribute);
                        var fromTable = fromAttribute.parentEntity.name;
                        var toTable = toAttribute.parentEntity.name;

                        string rel = "rel_" + fromTable + "_" + toTable;
                        str += "<div id='" + rel + "' class='drag' style='margin: 30px; float: left; width: 60px;     height: 60px;    background: #B97A57; /* Rotate */     -webkit-transform: rotate(-50deg);     -moz-transform: rotate(-50deg);     -ms-transform: rotate(-50deg);     -o-transform: rotate(-50deg);     transform: rotate(-50deg); /* Rotate Origin */     -webkit-transform-origin: 0 100%;     -moz-transform-origin: 0 100%;     -ms-transform-origin: 0 100%;     -o-transform-origin: 0 100%;     transform-origin: 0 100%; '>" + r.relationshipName + "<br/>" + r.type + "</div>";
                    }
                    else if (r.relationtype == "weak")
                    {
                        var fromAttribute = attributes.SingleOrDefault(a => a.givenId == r.primaryAttribute);
                        var toAttribute = attributes.SingleOrDefault(a => a.givenId == r.foriegnAttribute);
                        var fromTable = fromAttribute.parentEntity.name;
                        var toTable = toAttribute.parentEntity.name;

                        string rel = "rel_" + fromTable + "_" + toTable;
                        str += "<div id='" + rel + "' class='drag' style='margin: 30px; float: left; width: 60px;  border-top: 5px double ;border-right: 5px double ;border-bottom: 5px double ;border-left: 5px double ;   height: 60px; border-style: double;    background: #C57AC5; /* Rotate */     -webkit-transform: rotate(-50deg);     -moz-transform: rotate(-50deg);     -ms-transform: rotate(-50deg);     -o-transform: rotate(-50deg);     transform: rotate(-50deg); /* Rotate Origin */     -webkit-transform-origin: 0 100%;     -moz-transform-origin: 0 100%;     -ms-transform-origin: 0 100%;     -o-transform-origin: 0 100%;     transform-origin: 0 100%; '>" + r.relationshipName + "<br/>" + r.type + "</div>";
                    }
                    //    var fromAttribute = attributes.SingleOrDefault(a => a.givenId == r.primaryAttribute);
                    //    var toAttribute = attributes.SingleOrDefault(a => a.givenId == r.foriegnAttribute);
                    //    var fromTable = fromAttribute.parentEntity.name;
                    //    var toTable = toAttribute.parentEntity.name;

                    //    string rel = "rel_" + fromTable + "_" + toTable;
                    //    str += "<div id='" + rel + "' class='drag' style='margin: 30px; float: left; min-width: 60px;     min-height: 60px;     background: #B97A57; /* Rotate */     -webkit-transform: rotate(-50deg);     -moz-transform: rotate(-50deg);     -ms-transform: rotate(-50deg);     -o-transform: rotate(-50deg);     transform: rotate(-50deg); /* Rotate Origin */     -webkit-transform-origin: 0 100%;     -moz-transform-origin: 0 100%;     -ms-transform-origin: 0 100%;     -o-transform-origin: 0 100%;     transform-origin: 0 100%; '>" + r.relationshipName + "<br/>" + r.type + "</div>";
                    //}
                }

            }


            str += "</tr></table>";




            rightDiv.InnerHtml += str;

            //Draw lines
            foreach (var item in entities.Where(c => c.status != "Deleted").ToList())
            {
                foreach (var attr in item.childAttributes.Where(c => c.status != "Deleted").ToList())
                {
                    Page.Header.Controls.Add(new LiteralControl(
                                "<script>$(function(){$('#" + item.name + "_" + attr.name +
                                "').connections({to: '#" + item.name + "'});" +
                                "$.repeat().add('connection').each($).connections('update').wait(1);});" +
                                "</script>"));

                    if (attr.attributeType == "composite")
                    {
                        foreach (var ca in attr.compositeChildAttributes.Where(c => c.status != "Deleted").ToList())
                        {
                            Page.Header.Controls.Add(new LiteralControl(
                                "<script>$(function(){$('#" + ca.givenId +
                                "').connections({to: '#" + attr.givenId + "'});" +
                                "$.repeat().add('connection').each($).connections('update').wait(1);});" +
                                "</script>"));
                        }
                    }

                }
            }

            foreach (var item in relationships.Where(c => c.status != "Deleted").ToList())
            {
                var fromAttribute = attributes.SingleOrDefault(a => a.givenId == item.primaryAttribute);
                var toAttribute = attributes.SingleOrDefault(a => a.givenId == item.foriegnAttribute);
                var fromTable = fromAttribute.parentEntity.name;
                var toTable = toAttribute.parentEntity.name;

                string rel = "rel_" + fromTable + "_" + toTable;

                Page.Header.Controls.Add(new LiteralControl(
                                "<script>$(function(){$('#" + fromTable +
                                "').connections({to: '#" + rel + "'});" +
                                "$.repeat().add('connection').each($).connections('update').wait(0);});" +
                                "</script>"));

                Page.Header.Controls.Add(new LiteralControl(
                                "<script>$(function(){$('#" + rel +
                                "').connections({to: '#" + toTable + "'});" +
                                "$.repeat().add('connection').each($).connections('update').wait(0);});" +
                                "</script>"));
            }
        }

        protected void cancel_Click(object sender, EventArgs e)
        {
            drawDiagram();
        }

        protected void btnQueryExecute_Click(object sender, EventArgs e)
        {
            rightDiv.InnerHtml = "";

            var connectionString = @"Data Source=" + txtQueryHost.Text + ";Initial Catalog=" + txtQueryDatabase.Text + ";User ID=" + txtQueryUsername.Text + ";Password=" + txtQueryPassword.Text;
            //string connectionString = @"Data Source=.;Initial Catalog=DrawerDb;Integrated Security=True";

            string queryStr = txtQueryString.Text;

            string queryType = ddlQueryType.SelectedItem.Value;

            try
            {
                SqlConnection con = new SqlConnection(connectionString);

                SqlCommand cmd = new SqlCommand(queryStr, con);

                con.Open();

                if (queryType == "SELECT")
                {
                    SqlDataReader dr = cmd.ExecuteReader();

                    int columnsCount = dr.FieldCount;

                    rightDiv.InnerHtml += "<table>";

                    while (dr.Read())
                    {
                        string row = "<tr>";

                        for (int i = 0; i < columnsCount; i++)
                        {
                            row += "<td style='border: solid 1px black; width: 100px'>" + dr[i].ToString() + "</td>";
                        }

                        row += "</tr>";

                        rightDiv.InnerHtml += row;
                    }

                    rightDiv.InnerHtml += "</table>";
                }
                else
                {
                    int count = cmd.ExecuteNonQuery();

                    if (count != 0)
                    {
                        alertDiv.InnerHtml = "<div class='alert alert-success'><strong>Success!</strong> Relationship added successfully.</div>";
                    }
                    else
                    {
                        alertDiv.InnerHtml = "<div class='alert alert-danger'><strong>Error!</strong> Insert failed.</div>";
                    }
                }

                con.Close();
            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    case 4060: // Invalid Database 
                        alertDiv.InnerHtml = "<div class='alert alert-danger'><strong>Invalid database!</strong> Insert failed.</div>";
                        break;

                    case 18456: // Login Failed 

                        alertDiv.InnerHtml = "<div class='alert alert-danger'><strong>Invalid login!</strong> Insert failed.</div>";

                        break;

                    case 547: // ForeignKey Violation 

                        alertDiv.InnerHtml = "<div class='alert alert-danger'><strong>Forign key violation!</strong> Insert failed.</div>";

                        break;

                    case 2627:
                        // Unique Index/ Primary key Violation/ Constriant Violation 

                        alertDiv.InnerHtml = "<div class='alert alert-danger'><strong>Unique Index/ Primary key Violation/ Constriant Violation!</strong> Insert failed.</div>";

                        break;

                    case 2601: // Unique Index/Constriant Violation 
                        alertDiv.InnerHtml = "<div class='alert alert-danger'><strong>Unique Index/Constriant Violation!</strong> Insert failed.</div>";


                        break;

                    default:

                        alertDiv.InnerHtml = "<div class='alert alert-danger'><strong>Unknow error!</strong> Insert failed.</div>";


                        break;

                }
            }
            catch (Exception ex)
            {
                string error = ex.ToString();

                rightDiv.InnerText = error;
                //Message

                //throw;
            }

        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            entities = new List<EntityModel>();
            attributes = new List<AttributeModel>();
            relationships = new List<RelationshipModel>();
            compositeAttributeChilds = new List<CompositeChildAttributeModel>();
            Response.Redirect("DrawingPage.aspx");
        }

        public void clearForm()
        {
            this.txtAttributeName.Text = string.Empty;
            this.chkEditAttributeIsPK.Checked = false;
            this.ddlAttributeDataType.SelectedIndex = -1;
        }

        //public void drawDiagram()
        //{

        //    int columnsCount = 0;
        //    rightDiv.InnerHtml = "";
        //    string str = "<table><tr>";
        //    foreach (var item in entities.Where(c => c.status != "Deleted").ToList())
        //    {

        //        if (columnsCount == 3)
        //        {
        //            str += "</tr><tr>";
        //            columnsCount = 0;
        //        }

        //        str += "<td>";

        //        if (item.type == "normal")
        //        {
        //            str += "<div  class='drag' onmouseover='getPos(this)' style='top:" + item.xAxis + "px;left:" + item.yAxis + "px'><div id='" + item.name + "' style='width: 90px;min-height: 50px;flex-flow: column;background: #4B55CF;margin: 20px;text-align: center;'><b>";
        //        }
        //        else if (item.type == "weak")
        //        {
        //            str += "<div class='drag'  id='" + item.name + "' style='width: 90px;min-height: 50px;background: #FF9E5E;margin: 20px;text-align: center;vertical-align: middle;line-height: 40px;border-top: 5px double ;border-right: 5px double ;border-bottom: 5px double ;border-left: 5px double ;'><b>";

        //        }
        //        str += item.name;

        //        str += "</b></div>";
        //        str += "<br/>";
        //        foreach (var attr in item.childAttributes.Where(c => c.status != "Deleted").ToList())
        //        {
        //            if (attr.attributeType == "normal")
        //            {
        //                str += "<div class='drag' id='" + item.name + "_" + attr.name + "' style='min-width: 70px;min-height: 30px;background: #00B95C;-moz-border-radius: 70px /30px;-webkit-border-radius: 70px / 30px;border-radius: 70px / 30px;float: left;margin: 5px;margin-bottom: 25px;text-align: center;vertical-align: middle;line-height: 25px;'>";
        //            }
        //            else if (attr.attributeType == "derived")
        //            {
        //                str += "<div class='drag' id='" + item.name + "_" + attr.name + "' style='min-width: 70px;min-height: 30px;  -moz-border-radius: 70px / 30px;-webkit-border-radius: 70px / 30px;border-radius: 70px / 30px; background-color: #FFFFFF; border: 2px dotted #000000; -moz-border: 2px dotted #000000; text-align: center;float:left; margin: 5px; margin-bottom: 25px; vertical-align: middle; line-height: 25px;'>";
        //            }
        //            else if (attr.attributeType == "multi")
        //            {
        //                str += "<div class='drag' id='" + item.name + "_" + attr.name + "' style='min-width: 70px; flex: auto;min-height: 30px;  -moz-border-radius: 70px / 30px;-webkit-border-radius: 70px / 30px;border-radius: 70px / 30px; background-color: #99D9EA; border: 5px double #000000; -moz-border: 2px double #000000; text-align: center;float:left; margin: 5px; margin-bottom: 25px; vertical-align: middle; line-height: 25px;'>";
        //            }
        //            else if (attr.attributeType == "composite")
        //            {
        //                str += "<table><tr>";

        //                foreach (var ca in attr.compositeChildAttributes.Where(c => c.status != "Deleted").ToList())
        //                {
        //                    str += "<td><div class='drag' id='" + ca.givenId + "' style='width: 70px;height: 30px;background: #00B95C;-moz-border-radius: 70px /30px;-webkit-border-radius: 70px / 30px;border-radius: 70px / 30px;float: left;margin: 5px;margin-bottom: 25px;text-align: center;vertical-align: middle;line-height: 25px;'>" + ca.name + "</div></td>";
        //                }

        //                str += "</tr></table>";

        //                str += "<div class='drag' id='" + item.name + "_" + attr.name + "' style='width: 70px;height: 30px;background: #f9f902;-moz-border-radius: 70px /30px;-webkit-border-radius: 70px / 30px;border-radius: 70px / 30px;float: left;margin: 5px;margin-bottom: 25px;text-align: center;vertical-align: middle;line-height: 25px;'>";
        //            }


        //            if (attr.isKey)
        //            {
        //                str += "<u><b>";
        //            }

        //            str += attr.name;

        //            if (attr.isKey)
        //            {
        //                str += "</b></u>";
        //            }


        //            str += "</div>";
        //        }

        //        str += "</div></td>";

        //        columnsCount++;



        //        foreach (var r in item.chilRelationships.Where(c => c.status != "Deleted").ToList())
        //        {
        //            if (r.relationtype == "normal")
        //            {
        //                var fromAttribute = attributes.SingleOrDefault(a => a.givenId == r.primaryAttribute);
        //                var toAttribute = attributes.SingleOrDefault(a => a.givenId == r.foriegnAttribute);
        //                var fromTable = fromAttribute.parentEntity.name;
        //                var toTable = toAttribute.parentEntity.name;

        //                string rel = "rel_" + fromTable + "_" + toTable;
        //                str += "<div id='" + rel + "' class='drag' style='margin: 30px; float: left; width: 60px;     height: 60px;    background: #B97A57; /* Rotate */     -webkit-transform: rotate(-50deg);     -moz-transform: rotate(-50deg);     -ms-transform: rotate(-50deg);     -o-transform: rotate(-50deg);     transform: rotate(-50deg); /* Rotate Origin */     -webkit-transform-origin: 0 100%;     -moz-transform-origin: 0 100%;     -ms-transform-origin: 0 100%;     -o-transform-origin: 0 100%;     transform-origin: 0 100%; '>" + r.relationshipName + "<br/>" + r.type + "</div>";
        //            }
        //            else if (r.relationtype == "weak")
        //            {
        //                var fromAttribute = attributes.SingleOrDefault(a => a.givenId == r.primaryAttribute);
        //                var toAttribute = attributes.SingleOrDefault(a => a.givenId == r.foriegnAttribute);
        //                var fromTable = fromAttribute.parentEntity.name;
        //                var toTable = toAttribute.parentEntity.name;

        //                string rel = "rel_" + fromTable + "_" + toTable;
        //                str += "<div id='" + rel + "' class='drag' style='margin: 30px; float: left; width: 60px;  border-top: 5px double ;border-right: 5px double ;border-bottom: 5px double ;border-left: 5px double ;   height: 60px; border-style: double;    background: #C57AC5; /* Rotate */     -webkit-transform: rotate(-50deg);     -moz-transform: rotate(-50deg);     -ms-transform: rotate(-50deg);     -o-transform: rotate(-50deg);     transform: rotate(-50deg); /* Rotate Origin */     -webkit-transform-origin: 0 100%;     -moz-transform-origin: 0 100%;     -ms-transform-origin: 0 100%;     -o-transform-origin: 0 100%;     transform-origin: 0 100%; '>" + r.relationshipName + "<br/>" + r.type + "</div>";
        //            }
        //        }
        //    }


        //    str += "</tr></table>";

        //    rightDiv.InnerHtml += str;

        //    //Draw lines
        //    foreach (var item in entities)
        //    {
        //        foreach (var attr in item.childAttributes.Where(c => c.status != "Deleted").ToList())
        //        {
        //            Page.Header.Controls.Add(new LiteralControl(
        //                        "<script>$(function(){$('#" + item.name + "_" + attr.name +
        //                        "').connections({to: '#" + item.name + "'});" +
        //                        "$.repeat().add('connection').each($).connections('update').wait(1);});" +
        //                        "</script>"));
        //        }
        //    }

        //    foreach (var item in relationships.Where(c => c.status != "Deleted").ToList())
        //    {
        //        var fromAttribute = attributes.SingleOrDefault(a => a.givenId == item.primaryAttribute);
        //        var toAttribute = attributes.SingleOrDefault(a => a.givenId == item.foriegnAttribute);
        //        var fromTable = fromAttribute.parentEntity.name;
        //        var toTable = toAttribute.parentEntity.name;

        //        string rel = "rel_" + fromTable + "_" + toTable;

        //        Page.Header.Controls.Add(new LiteralControl(
        //                        "<script>$(function(){$('#" + fromTable +
        //                        "').connections({to: '#" + rel + "'});" +
        //                        "$.repeat().add('connection').each($).connections('update').wait(0);});" +
        //                        "</script>"));

        //        Page.Header.Controls.Add(new LiteralControl(
        //                        "<script>$(function(){$('#" + rel +
        //                        "').connections({to: '#" + toTable + "'});" +
        //                        "$.repeat().add('connection').each($).connections('update').wait(0);});" +
        //                        "</script>"));
        //    }
        //}
    }
}