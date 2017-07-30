<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DrawingPage.aspx.cs" Inherits="Drawer.DrawingPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Includes/bootstrap.min.css" rel="stylesheet" />
    <script src="Includes/jquery-1.12.4.js"></script>
    <script src="Includes/jquery-ui.js"></script>
    <script src="Includes/jquery.connections.js"></script>
    <script src="Includes/jquery-timing.min.js"></script>
    <script src="Includes/jquery.ui.touch-punch.min.js"></script>
    <script src="Includes/bootstrap.min.js"></script>
    <link href="Includes/jquery-ui.css" rel="stylesheet" />


    <style>
        .left {
            position: relative;
            top: 4px;
            float: left;
            width: 10%;
            height: 1000px;
            padding: 2px;
            border: solid 2px #a5a1a1;
            border-right: solid 5px #a5a1a1;
        }

        .right {
            position: relative;
            top: 4px;
            float: right;
            width: 89%;
            height: 1000px;
            border: solid 2px #a5a1a1;
            border-left: solid 5px #a5a1a1;
            padding: 3px;
        }

        .top {
            height: 50px;
            border: solid 2px #a5a1a1;
            border-bottom: 5px solid #a5a1a1;
            border-top: 5px solid #a5a1a1;
            background-color: #d8c5c5;
            padding: 0px;

        }
       
        .logo{
            position: relative;
            top: 4px;
            float: right;
        
        }

        .content {
            position: relative;
            left: 400px;
        }

        #open {
            position: relative;
            left: 40px;
        }

        #new {
            position: relative;
            left: 70px;
        }

        #save {
            position: relative;
            left: 100px;
        }

        #cut {
            position: relative;
            left: 130px;
        }

        #undo {
            position: relative;
            left: 160px;
        }

        #redo {
            position: relative;
            left: 190px;
        }

        .block {
            width: 64px;
            height: 64px;
            background: white;
            border: 1px solid black;
            border-radius: 15px;
        }

        connection {
            border: 3px solid black;
            border-radius: 31px;
            z-index: -999;
        }

        .auto-style1 {
            height: 90px;
        }

        .auto-style2 {
            width: 45px;
        }

        .auto-style3 {
            height: 90px;
            width: 45px;
        }

        .modal .modal-body {
            max-height: 500px;
            overflow-y: auto;
        }

        .tooltip {
            color: #900;
            text-decoration: none;
            }

            .tooltip:hover {
            color: red;
            position: relative;
            }

            /* Tooltip on Top */
            .tooltip-top[data-tooltip]:hover:after {
            content: attr(data-tooltip);
            padding: 4px 8px;
            position: absolute;
            left: 0;
            bottom: 100%;
            white-space: nowrap;
            z-index: 20px;

            background-color: #000;
            color: #fff;
        }
    </style>


    <link href="Shapes.css" rel="stylesheet" />
    <script>
        $(function () {
            $(".drag").draggable({ containment: "#rightDiv", scroll: false });
        });
    </script>


    <%-- <script>
        function ddl_change(value) {
            var lbl = document.getElementById('<%= txtEditAttributeNewName.ClientID %>');
            lbl.value = value;
        }
    </script>--%>
</head>
<body>
    <form id="form1" runat="server">
        <div class="top">
           
            <a href="#" data-toggle="modal" data-target="#modalNew" >
                <img src="imgs/default.png" alt="New" style="margin-left: 15px; width: 40px; height: 40px ; " /></a>
            <a href="#" data-toggle="modal" data-target="#modalSaveDiagram">
                <img src="imgs/floppy-512.png" alt="Save" style="margin-left: 25px; width: 40px; height: 40px" /></a>
            <a href="#" data-toggle="modal" data-target="#modalOpenDiagram">
                <img src="imgs/system-pictures-icon.png" alt="Open Diagram" style="margin-left: 25px; width: 40px; height: 40px" /></a>
            <a href="#" data-toggle="modal" data-target="#modalGenerateDatabase">
                <img src="imgs/database_wizard.png" alt="Generate" style="margin-left: 25px; width: 40px; height: 40px" /></a>
            <a href="#" data-toggle="modal" data-target="#modalQuery">
                <img src="imgs/query.png" alt="TestScreen" style="margin-left: 25px; width: 40px; height: 40px" /></a>
            <a href="#" data-toggle="modal" data-target="#modalHelp">
                <img src="imgs/FAQ.png" alt="Help" style="margin-left: 25px; width: 40px; height: 40px" /></a>
            <div class="logo">
                <h2 style="margin-top:0px;margin-right:10px">
                    <table><tr>
                    <td style="color:#4B55CF">A</td>
                    <td style="color:rgb(255, 158, 94)">D</td>
                    <td style="color:rgb(0, 185, 92)">B</td>
                    <td style="color:yellow">G</td> 

                                                  </tr></table></h2>
            </div>
        </div>
        <asp:HiddenField ID="hfxCordinate" runat="server" />
        <asp:HiddenField ID="hfyCordinate" runat="server" />

        <div id="alertDiv" runat="server"></div>
        <div class="left">
            <table>
                <tr>
                    <td>
                        <a href="#" data-toggle="modal" data-target="#modalAddEntity">
                            <img src="imgs/Entity - Copy.PNG" alt="New Entity" height="80" width="80" style="margin-left: 15px; width: 90px; height: 90px" />
                        </a>
                    </td>
                    <td class="auto-style2">
                        <a href="#" data-toggle="modal" data-target="#modalEditEntity">
                            <img src="imgs/edit2.png" alt="Edit Entity" style="margin-left: 15px; width: 30px; height: 30px" />
                        </a>
                    </td>
                </tr>

                <tr>
                    <td>
                        <a href="#" data-toggle="modal" data-target="#modalAddWeakEntity">
                            <img src="imgs/weakrelcap - Copy.PNG" alt="New Entity" height="80" width="80" style="margin-left: 15px; width: 90px; height: 90px" />
                        </a>
                    </td>
                    <td class="auto-style2">
                        <a href="#" data-toggle="modal" data-target="#modalEditWeakEntity">
                            <img src="imgs/edit2.png" alt="Edit Entity" style="margin-left: 15px; width: 30px; height: 30px" />
                        </a>
                    </td>
                </tr>

                <tr>
                    <td><a href="#" data-toggle="modal" data-target="#modalAddAttribute">
                        <img src="imgs/attributecap - Copy.PNG" alt="New Attribute" height="80" width="80" style="margin-left: 15px; width: 90px; height: 90px" />
                    </a>
                    </td>
                    <td class="auto-style2">
                        <a href="#" data-toggle="modal" data-target="#modalEditAttribute">
                            <img src="imgs/edit2.png" alt="Edit Entity" style="margin-left: 15px; width: 30px; height: 30px" />
                        </a>
                    </td>
                </tr>

                <tr>
                    <td><a href="#" data-toggle="modal" data-target="#modalAddDrivedAttribute">
                        <img src="imgs/derivedcap - Copy.PNG" alt="New Attribute" height="80" width="80" style="margin-left: 15px; width: 90px; height: 90px" />
                    </a>
                    </td>
                    <td class="auto-style2">
                        <a href="#" data-toggle="modal" data-target="#modalEditDrivedAttribute">
                            <img src="imgs/edit2.png" alt="Edit Entity" style="margin-left: 15px; width: 30px; height: 30px" />
                        </a>
                    </td>
                </tr>

                <tr>
                    <td><a href="#" data-toggle="modal" data-target="#modalAddCompositeAttribute">
                        <img src="imgs/compocap - Copy.PNG" alt="New Multi Attribute" height="80" width="80" style="margin-left: 15px; width: 90px; height: 90px" />
                    </a>
                    </td>
                    <td class="auto-style2">
                        <a href="#" data-toggle="modal" data-target="#modalEditCompositeAttribute">
                            <img src="imgs/edit2.png" alt="Edit Entity" style="margin-left: 15px; width: 30px; height: 30px" />
                        </a>
                    </td>
                </tr>

                <tr>
                    <td><a href="#" data-toggle="modal" data-target="#modalAddMultiValuedAttribute">
                        <img src="imgs/multivalcap - Copy.PNG" alt="New Composite Attribute" height="80" width="80" style="margin-left: 15px; width: 90px; height: 90px" />
                    </a>
                    </td>
                    <td class="auto-style2">
                        <a href="#" data-toggle="modal" data-target="#modalEditMultiValuedAttribute">
                            <img src="imgs/edit2.png" alt="Edit Entity" style="margin-left: 15px; width: 30px; height: 30px" />
                        </a>
                    </td>
                </tr>

                <tr>
                    <td><a href="#" data-toggle="modal" data-target="#modalAddRelationship">
                        <img src="imgs/relacap - Copy.PNG" alt="New Attribute" height="80" width="80" style="margin-left: 15px; width: 90px; height: 90px" />
                    </a></td>
                    <td class="auto-style2">
                        <a href="#" data-toggle="modal" data-target="#modalEditRelationship">
                            <img src="imgs/edit2.png" alt="Edit Entity" style="margin-left: 15px; width: 30px; height: 30px" />
                        </a>
                    </td>
                </tr>

                <tr>
                    <td class="auto-style1"><a href="#" data-toggle="modal" data-target="#modalAddWeakRelationship">
                        <img src="imgs/weeeeeak - Copy.PNG" alt="New Weak Action" height="80" width="80" style="margin-left: 15px; width: 90px; height: 90px" />
                    </a></td>
                    <td class="auto-style3">
                        <a href="#" data-toggle="modal" data-target="#modalEditWeakRelationship">
                            <img src="imgs/edit2.png" alt="Edit Entity" style="margin-left: 15px; width: 30px; height: 30px" />
                        </a>
                    </td>
                </tr>
            </table>


        </div>

        <div id="rightDiv" class="right" runat="server"></div>

        <div id="modalNew" class="modal fade" role="dialog">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">New Diagram</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form" role="form">
                            <div class="form-group">
                                <p>Clear screen and start new model?</p>                               
                            </div>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <asp:Button ID="btnNew" Text="Confirm" runat="server" CssClass="btn btn-default" OnClick="btnNew_Click" />
                        <asp:Button ID="btnNewCancel" Text="Cancel" runat="server" CssClass="btn btn-default" OnClick="cancel_Click" />

                    </div>
                </div>
            </div>
        </div>


        <div id="modalAddEntity" class="modal fade" role="dialog">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Entity</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form" role="form">
                            <div class="form-group">
                                <label for="entityName">Entity Name:</label>
                                <asp:TextBox ID="txtEntityName" name="entityName" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <asp:Button ID="btnAddEntity" Text="Insert" runat="server" CssClass="btn btn-default" OnClick="btnAddEntity_Click"/>
                        <asp:Button ID="Button1" Text="Cancel" runat="server" CssClass="btn btn-default" OnClick="cancel_Click" />

                    </div>
                </div>
            </div>
        </div>

        <div id="modalEditEntity" class="modal fade" role="dialog">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Edit Entity</h4>
                    </div>
                    <div class="modal-body">
                        <div class="tabbable">
                            <ul class="nav nav-tabs">
                                <li class="active"><a href="#EntityName" data-toggle="tab">Update</a></li>
                                <li><a href="#DeleteEntity" data-toggle="tab">Delete</a></li>

                            </ul>
                            <div class="tab-content">
                                <div class="tab-pane active" id="EntityName">
                                    <div class="form" role="form">
                                        <div class="form-group">
                                            <label for="editEntity">Select Entity:</label>
                                            <asp:DropDownList ID="ddlEditEntitySelect" name="editEntity" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlEditEntitySelect_SelectedIndexChanged">
                                                <asp:ListItem Text="-- Select Entity --" />
                                            </asp:DropDownList>
                                        </div>

                                        <div class="form-group">
                                            <label for="entityName">New Name:</label>
                                            <asp:TextBox ID="txtEditEntityNewName" name="entityName" runat="server" CssClass="form-control" />
                                        </div>
                                        <div class="form-group">
                                            <asp:Button ID="btnEditEntity" Text="Modify" runat="server" CssClass="btn btn-default" OnClick="btnEditEntity_Click" />
                                        </div>
                                    </div>
                                </div>
                                <div class="tab-pane" id="DeleteEntity">
                                    <div class="form" role="form">
                                        <div class="form-group">
                                            <label for="attributeEntity">Entity Name:</label>
                                            <asp:DropDownList ID="ddlDeleteEntity" name="attributeEntity" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="-- Select Entity --" />
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group">
                                            <asp:Button ID="btnDeleteEntity" Text="Delete" runat="server" CssClass="btn btn-default" OnClick="btnDeleteEntity_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>


                    <div class="modal-footer">
                        <asp:Button ID="btnCancelEditEntity" Text="Cancel" runat="server" CssClass="btn btn-default" OnClick="cancel_Click" />
                    </div>
                </div>
            </div>
        </div>

        <div id="modalAddWeakEntity" class="modal fade" role="dialog">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Weak Entity</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form" role="form">
                            <div class="form-group">
                                <label for="entityName">Weak Entity Name:</label>
                                <asp:TextBox ID="txtWeakEntityName" name="entityName" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <asp:Button ID="btnAddWeakEntity" Text="Insert" runat="server" CssClass="btn btn-default" OnClick="btnAddWeakEntity_Click" />
                        <asp:Button ID="Button2" Text="Cancel" runat="server" CssClass="btn btn-default" OnClick="cancel_Click" />
                    </div>
                </div>
            </div>
        </div>

        <div id="modalEditWeakEntity" class="modal fade" role="dialog">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Edit Entity</h4>
                    </div>
                    <div class="modal-body">
                        <div class="tabbable">
                            <ul class="nav nav-tabs">
                                <li class="active"><a href="#UpdateWEakEntity" data-toggle="tab">Update</a></li>
                                <li><a href="#DeleteWeakEntity" data-toggle="tab">Delete</a></li>

                            </ul>
                            <div class="tab-content">
                                <div class="tab-pane active" id="UpdateWEakEntity">
                                    <div class="form" role="form">
                                        <div class="form-group">
                                            <label for="editEntity">Select Entity:</label>
                                            <asp:DropDownList ID="ddlEditWeakEntitySelect" name="editEntity" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlEditEntitySelect_SelectedIndexChanged">
                                                <asp:ListItem Text="-- Select Entity --" />
                                            </asp:DropDownList>
                                        </div>

                                        <div class="form-group">
                                            <label for="entityName">New Name:</label>
                                            <asp:TextBox ID="txtEditWeakEntityNewName" name="entityName" runat="server" CssClass="form-control" />
                                        </div>
                                        <div class="form-group">
                                            <asp:Button ID="Button13" Text="Modify" runat="server" CssClass="btn btn-default" OnClick="btnEditWeakEntity_Click" />
                                        </div>


                                    </div>
                                </div>
                                <div class="tab-pane" id="DeleteWeakEntity">
                                    <div class="form" role="form">
                                        <div class="form-group">
                                            <label for="attributeEntity">Weak Entity Name:</label>
                                            <asp:DropDownList ID="ddlDeleteWeakEntity" name="attributeEntity" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="-- Select Entity --" />
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group">
                                            <asp:Button ID="Button22" Text="Delete" runat="server" CssClass="btn btn-default" OnClick="btnDeleteWeakEntity_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">

                        <asp:Button ID="Button14" Text="Cancel" runat="server" CssClass="btn btn-default" OnClick="cancel_Click" />
                    </div>
                </div>
            </div>
        </div>

        <div id="modalAddAttribute" class="modal fade" role="dialog">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Attribute</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form" role="form">
                            <div class="form-group">
                                <label for="attributeName">Attribute Name:</label>
                                <asp:TextBox ID="txtAttributeName" name="attributeName" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <label for="attributeEntity">Entity Name:</label>
                                <asp:DropDownList ID="ddlAttributeEntity" name="attributeEntity" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="-- Select Entity --" />
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="attributeDataType">Data Type:</label>
                                <asp:DropDownList ID="ddlAttributeDataType" name="attributeDataType" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                    <asp:ListItem Text="-- Select Data Type --" Value="" />
                                    <asp:ListItem Text="Int" Value="Int" />
                                    <asp:ListItem Text="Nvarchar" Value="Nvarchar" />
                                    <asp:ListItem Text="Float" Value="Float" />
                                    <asp:ListItem Text="Bit (Boolean)" Value="Bit" />
                                    <asp:ListItem Text="Date" Value="Date" />
                                    <asp:ListItem Text="DateTime" Value="DateTime" />                                   
                                    <asp:ListItem Text="Binary" Value="Binary" />
                                    <asp:ListItem Text="Char" Value="Char" />

                                    
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="attributeLength">Attribute Length:</label>
                                <asp:TextBox ID="txtAttributeLength" name="attributeLength" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <asp:CheckBox ID="cbIsPK" Text="   Is Primary Key?" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <asp:Button ID="btnAddAtribute" Text="Insert" runat="server" CssClass="btn btn-default" OnClick="btnAddAtribute_Click" />
                        <asp:Button ID="Button3" Text="Cancel" runat="server" CssClass="btn btn-default" OnClick="cancel_Click" />
                    </div>
                </div>
            </div>
        </div>

        <div id="modalEditAttribute" class="modal fade" role="dialog">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Edit Attribute</h4>
                    </div>
                    <div class="modal-body">
                        <div class="tabbable">
                            <ul class="nav nav-tabs">
                                <li class="active"><a href="#UpdateAttribute" data-toggle="tab">Update</a></li>
                                <li><a href="#DeleteAttribute" data-toggle="tab">Delete</a></li>

                            </ul>
                            <div class="tab-content">
                                <div class="tab-pane active" id="UpdateAttribute">
                                    <div class="form" role="form">
                                        <div class="form-group">
                                            <label for="editEntity">Select Attribute:</label>
                                            <asp:DropDownList ID="ddlEditAttributeSelectAttribute" name="editEntity" runat="server" CssClass="form-control">                                              
                                                <asp:ListItem Text="-- Select Entity --" />
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group">
                                            <label for="attributeName">Attribute new Name:</label>
                                            <asp:TextBox ID="txtEditAttributeNewName" name="attributeName" runat="server" CssClass="form-control" />
                                        </div>
                                        <div class="form-group">
                                            <label for="attributeDataType">New Data Type:</label>
                                            <asp:DropDownList ID="ddlEditAttributeNewDataType" name="attributeDataType" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                                <asp:ListItem Text="-- Select Data Type --" />
                                                <asp:ListItem Text="Int" Value="Int" />
                                                <asp:ListItem Text="Nvarchar" Value="Nvarchar" />
                                                <asp:ListItem Text="Float" Value="Float" />
                                                <asp:ListItem Text="Bit" Value="Bit" />
                                                 <asp:ListItem Text="Bit" Value="Date" />
                                                <asp:ListItem Text="Bit" Value="Boolean" />
                                                <asp:ListItem Text="Bit" Value="Binary" />
                                                 <asp:ListItem Text="Bit" Value="Char" />
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group">
                                            <label for="attributeLength">New Attribute Length:</label>
                                            <asp:TextBox ID="txtEditAttributeNewLength" name="attributeLength" runat="server" CssClass="form-control" />
                                        </div>
                                        <div class="form-group">
                                            <asp:CheckBox ID="chkEditAttributeIsPK" Text="   Is Primary Key?" runat="server" CssClass="form-control" />
                                        </div>
                                        <div class="form-group">
                                            <asp:Button ID="btnEditAttributeSave" Text="Modify" runat="server" CssClass="btn btn-default" OnClick="btnEditAttributeSave_Click" />
                                        </div>
                                    </div>
                                </div>
                                <div class="tab-pane" id="DeleteAttribute">
                                    <div class="form" role="form">
                                        <div class="form-group">
                                            <label for="attributeEntity">Select Attribute:</label>
                                            <asp:DropDownList ID="ddlDeleteAttribute" name="attributeEntity" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="-- Select Attribute --" />
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group">
                                            <asp:Button ID="btnDeleteAttribute" Text="Delete" runat="server" CssClass="btn btn-default" OnClick="btnDeleteAttribute_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <asp:Button ID="Button4" Text="Cancel" runat="server" CssClass="btn btn-default" OnClick="cancel_Click" />
                    </div>
                </div>
            </div>
        </div>

        <div id="modalAddDrivedAttribute" class="modal fade" role="dialog">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Derived Attribute</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form" role="form">
                            <div class="form-group">
                                <label for="drivedAttributeName">Derived Attribute Name:</label>
                                <asp:TextBox ID="txtDrivedAttributeName" name="drivedAttributeName" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <label for="drivedAttributeEntity">Entity Name:</label>
                                <asp:DropDownList ID="ddlDrivedAttributeEntity" name="drivedAttributeEntity" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="-- Select Entity --" />
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="drivedAttributeDataType">Data Type:</label>
                                <asp:DropDownList ID="ddlDrivedAttributeDataType" name="drivedAttributeDataType" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                    <asp:ListItem Text="-- Select Data Type --" />
                                    <asp:ListItem Text="Int" Value="Int" />
                                    <asp:ListItem Text="Nvarchar" Value="Nvarchar" />
                                    <asp:ListItem Text="Float" Value="Float" />
                                    <asp:ListItem Text="Bit" Value="Bit" />
                                    <asp:ListItem Text="DateTime" Value="DateTime" />
                                     <asp:ListItem Text="Bit" Value="Date" />
                                    <asp:ListItem Text="Bit" Value="Boolean" />
                                    <asp:ListItem Text="Bit" Value="Binary" />
                                     <asp:ListItem Text="Bit" Value="Char" />
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="drivedAttributeLength">Derived Attribute Length:</label>
                                <asp:TextBox ID="txtDrivedAttributeLength" name="drivedAttributeLength" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <asp:Button ID="btnDrivedAddAtribute" Text="Insert" runat="server" CssClass="btn btn-default" OnClick="btnAddDrivedAtribute_Click" />
                        <asp:Button ID="Button5" Text="Cancel" runat="server" CssClass="btn btn-default" OnClick="cancel_Click" />
                    </div>
                </div>
            </div>
        </div>

        <div id="modalEditDrivedAttribute" class="modal fade" role="dialog">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Edit Attribute</h4>
                    </div>
                    <div class="modal-body">
                        <div class="tabbable">
                            <ul class="nav nav-tabs">
                                <li class="active"><a href="#UpdateDrivedAttribute" data-toggle="tab">Update</a></li>
                                <li><a href="#DeleteDrivedAttribute" data-toggle="tab">Delete</a></li>

                            </ul>
                            <div class="tab-content">
                                <div class="tab-pane active" id="UpdateDrivedAttribute">
                                    <div class="form" role="form">

                                        <div class="form-group">
                                            <label for="editEntity">Select Attribute:</label>
                                            <asp:DropDownList ID="ddlEditDrivedAttributeSelectAttribute" name="editEntity" AppendDataBoundItems="true" runat="server" CssClass="form-control">
                                                <%--onchange="ddl_change(this.value)">--%>
                                                <asp:ListItem Text="-- Select Attribute --" />
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group">
                                            <label for="attributeName">Attribute new Name:</label>
                                            <asp:TextBox ID="txtEditDrivedAttributeNewName" name="attributeName" runat="server" CssClass="form-control" />
                                        </div>
                                        <div class="form-group">
                                            <label for="attributeDataType">New Data Type:</label>
                                            <asp:DropDownList ID="ddlEditDrivedAttributeNewDataType" name="attributeDataType" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                                <asp:ListItem Text="-- Select Data Type --" />
                                                <asp:ListItem Text="Int" Value="Int" />
                                                <asp:ListItem Text="Nvarchar" Value="Nvarchar" />
                                                <asp:ListItem Text="Float" Value="Float" />
                                                <asp:ListItem Text="Bit" Value="Bit" />
                                                 <asp:ListItem Text="Bit" Value="Date" />
                                                <asp:ListItem Text="Bit" Value="Boolean" />
                                                <asp:ListItem Text="Bit" Value="Binary" />
                                                 <asp:ListItem Text="Bit" Value="Char" />
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group">
                                            <label for="attributeLength">New Attribute Length:</label>
                                            <asp:TextBox ID="txtEditDrivedAttributeNewLength" name="attributeLength" runat="server" CssClass="form-control" />
                                        </div>
                                        <div class="form-group">
                                            <asp:Button ID="Button15" Text="Modify" runat="server" CssClass="btn btn-default" OnClick="btnEditDrivedAttributeSave_Click" />
                                        </div>
                                    </div>
                                </div>
                                <div class="tab-pane" id="DeleteDrivedAttribute">
                                    <div class="form" role="form">
                                        <div class="form-group">
                                            <label for="attributeEntity">Derived Attribute Name:</label>
                                            <asp:DropDownList ID="ddlDeleteDerivedAttribute" name="attributeEntity" runat="server" CssClass="form-control" >
                                                <asp:ListItem Text="-- Select Attribute --" />
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group">
                                            <asp:Button ID="Button24" Text="Delete" runat="server" CssClass="btn btn-default" onclick="btnDeleteDerivedAttribute_Click"/>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="modal-footer">

                        <asp:Button ID="Button16" Text="Cancel" runat="server" CssClass="btn btn-default" OnClick="cancel_Click" />
                    </div>
                </div>
            </div>
        </div>

        <div id="modalAddCompositeAttribute" class="modal fade" role="dialog">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Attribute</h4>
                    </div>
                    <div class="modal-body">

                        <div class="tabbable">
                            <ul class="nav nav-tabs">
                                <li class="active"><a href="#CompositeAttribute" data-toggle="tab">Attribute</a></li>
                                <li><a href="#ChildCompositAttribute" data-toggle="tab">Child Attribute</a></li>
                            </ul>
                            <div class="tab-content">
                                <div class="tab-pane active" id="CompositeAttribute">
                                    <div class="form" role="form">
                                        <div class="form-group">
                                            <label for="attributeName">Attribute Name:</label>
                                            <asp:TextBox ID="txtCompositeAttributeName" name="attributeName" runat="server" CssClass="form-control" />
                                        </div>
                                        <div class="form-group">
                                            <label for="attributeEntity">Parent Entity:</label>
                                            <asp:DropDownList ID="ddlCompositeAttributeEntity" name="attributeEntity" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="-- Select Entity --" />
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group">
                                            <asp:Button ID="btnAddCompositeAtribute" Text="Insert" runat="server" CssClass="btn btn-default" OnClick="btnAddCompositeAtribute_Click" />
                                        </div>
                                    </div>
                                </div>
                                <div class="tab-pane" id="ChildCompositAttribute">
                                    <div class="form" role="form">
                                        <div class="form-group">
                                            <label for="attributeName">Child Attribute Name:</label>
                                            <asp:TextBox ID="txtChilAttributeName" name="attributeName" runat="server" CssClass="form-control" />
                                        </div>
                                        <div class="form-group">
                                            <label for="attributeEntity">Composite Attribute Name:</label>
                                            <asp:DropDownList ID="ddlChildCompositeAttribute" name="attributeEntity" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="-- Select Composite Attribute --" />
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group">
                                            <label for="attributeDataType">Data Type:</label>
                                            <asp:DropDownList ID="ddlChildAttributeDataType" name="attributeDataType" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                                <asp:ListItem Text="-- Select Data Type --" />
                                                <asp:ListItem Text="Int" Value="Int" />
                                                <asp:ListItem Text="Nvarchar" Value="Nvarchar" />
                                                <asp:ListItem Text="Float" Value="Float" />
                                                <asp:ListItem Text="Bit" Value="Bit" />
                                                 <asp:ListItem Text="Bit" Value="Date" />
                                                 <asp:ListItem Text="Bit" Value="Boolean" />
                                                <asp:ListItem Text="Bit" Value="Binary" />
                                                 <asp:ListItem Text="Bit" Value="Char" />
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group">
                                            <label for="attributeLength">Attribute Length:</label>
                                            <asp:TextBox ID="txtChildAttributeLength" name="attributeLength" runat="server" CssClass="form-control" />
                                        </div>
                                        <div class="form-group">
                                            <asp:Button ID="btnAddCompositeChildAtribute" Text="Insert" runat="server" CssClass="btn btn-default" OnClick="btnAddCompositeChildAtribute_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="Button11" Text="Cancel" runat="server" CssClass="btn btn-default" OnClick="cancel_Click" />
                    </div>
                </div>
            </div>
        </div>

        <div id="modalEditCompositeAttribute" class="modal fade" role="dialog">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Edit Attribute</h4>
                    </div>
                    <div class="modal-body">
                        <div class="tabbable">
                            <ul class="nav nav-tabs">
                                <li class="active"><a href="#UpdateCompositeAttribute" data-toggle="tab">Update</a></li>
                                <li><a href="#DeleteCompositeAttribute" data-toggle="tab">Delete</a></li>

                            </ul>
                            <div class="tab-content">
                                <div class="tab-pane active" id="UpdateCompositeAttribute">
                                    <div class="form" role="form">

                                        <div class="form-group">
                                            <label for="editEntity">Select Attribute:</label>
                                            <asp:DropDownList ID="ddlEditComositeAttributeSelectAttribute" name="editEntity" AppendDataBoundItems="true" runat="server" CssClass="form-control">
                                                <%--onchange="ddl_change(this.value)">--%>
                                                <asp:ListItem Text="-- Select Attribute --" />
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group">
                                            <label for="attributeName">Attribute new Name:</label>
                                            <asp:TextBox ID="txtEditCompositeAttributeNewName" name="attributeName" runat="server" CssClass="form-control" />
                                        </div>
                                        <div class="form-group">
                                            <label for="attributeDataType">New Data Type:</label>
                                            <asp:DropDownList ID="ddlEditCompositeAttributeNewDataType" name="attributeDataType" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                                <asp:ListItem Text="-- Select Data Type --" />
                                                <asp:ListItem Text="Int" Value="Int" />
                                                <asp:ListItem Text="Nvarchar" Value="Nvarchar" />
                                                <asp:ListItem Text="Float" Value="Float" />
                                                <asp:ListItem Text="Bit" Value="Bit" />
                                                 <asp:ListItem Text="Bit" Value="Date" />
                                                 <asp:ListItem Text="Bit" Value="Boolean" />
                                                <asp:ListItem Text="Bit" Value="Binary" />
                                                 <asp:ListItem Text="Bit" Value="Char" />
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group">
                                            <label for="attributeLength">New Attribute Length:</label>
                                            <asp:TextBox ID="txtEditCompositeAttributeNewLength" name="attributeLength" runat="server" CssClass="form-control" />
                                        </div>
                                        <div class="form-group">
                                            <asp:Button ID="Button21" Text="Modify" runat="server" CssClass="btn btn-default" OnClick="btnEditCompositeAttributeSave_Click" />
                                        </div>
                                    </div>
                                </div>
                                <div class="tab-pane" id="DeleteCompositeAttribute">
                                    <div class="form" role="form">
                                        <div class="form-group">
                                            <label for="attributeEntity">Composite Attribute Name:</label>
                                            <asp:DropDownList ID="ddlDeleteCompositeAttribute" name="attributeEntity" runat="server" CssClass="form-control" >
                                                <asp:ListItem Text="-- Select Attribute --" />
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group">
                                            <asp:Button ID="Button23" Text="Delete" runat="server" CssClass="btn btn-default" onclick="btnDeleteCompositeAttribute_Click"/>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="modal-footer">

                        <asp:Button ID="Button25" Text="Cancel" runat="server" CssClass="btn btn-default" OnClick="cancel_Click" />
                    </div>
                </div>
            </div>
        </div>

        <div id="modalAddMultiValuedAttribute" class="modal fade" role="dialog">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Multivalued Attribute</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form" role="form">
                            <div class="form-group">
                                <label for="MultiValuedAttributeName">Multivalued Attribute Name:</label>
                                <asp:TextBox ID="txtMultiValuedAttributeName" name="MultiValuedAttributeName" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <label for="drivedAttributeEntity">Entity Name:</label>
                                <asp:DropDownList ID="ddlMultiValuedAttributeEntity" name="drivedAttributeEntity" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="-- Select Entity --" />
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="drivedAttributeDataType">Data Type:</label>
                                <asp:DropDownList ID="ddlMultiValuedAttributeDataType" name="drivedAttributeDataType" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                    <asp:ListItem Text="-- Select Data Type --" />
                                    <asp:ListItem Text="Int" Value="Int" />
                                    <asp:ListItem Text="Nvarchar" Value="Nvarchar" />
                                    <asp:ListItem Text="Float" Value="Float" />
                                    <asp:ListItem Text="Bit" Value="Bit" />
                                    <%--<asp:ListItem Text="DateTime" Value="DateTime" />--%>
                                     <asp:ListItem Text="Bit" Value="Date" />
                                    <asp:ListItem Text="Bit" Value="Boolean" />
                                    <asp:ListItem Text="Bit" Value="Binary" />
                                     <asp:ListItem Text="Bit" Value="Char" />
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="drivedAttributeLength">Multivalued Attribute Length:</label>
                                <asp:TextBox ID="txtMultiValuedAttributeLength" name="drivedAttributeLength" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <asp:Button ID="btnAddMultiValuedAtribute" Text="Insert" runat="server" CssClass="btn btn-default" OnClick="btnAddMultiValuedAtribute_Click" />
                        <asp:Button ID="Button6" Text="Cancel" runat="server" CssClass="btn btn-default" OnClick="cancel_Click" />
                    </div>
                </div>
            </div>
        </div>

        <div id="modalEditMultiValuedAttribute" class="modal fade" role="dialog">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Edit Attribute</h4>
                    </div>
                    <div class="modal-body">
                        <div class="tabbable">
                            <ul class="nav nav-tabs">
                                <li class="active"><a href="#UpdateMultiValuedAttribute" data-toggle="tab">Update</a></li>
                                <li><a href="#DeleteMultiValuedAttribute" data-toggle="tab">Delete</a></li>

                            </ul>
                            <div class="tab-content">
                                <div class="tab-pane active" id="UpdateMultiValuedAttribute">
                                    <div class="form" role="form">

                                        <div class="form-group">
                                            <label for="editEntity">Select Attribute:</label>
                                            <asp:DropDownList ID="ddlEditMultiValuedAttributeSelectAttribute" name="editEntity" AppendDataBoundItems="true" runat="server" CssClass="form-control">
                                                <%--onchange="ddl_change(this.value)">--%>
                                                <asp:ListItem Text="-- Select Attribute --" />
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group">
                                            <label for="attributeName">Attribute new Name:</label>
                                            <asp:TextBox ID="txtEditMultiValuedAttributeNewName" name="attributeName" runat="server" CssClass="form-control" />
                                        </div>
                                        <div class="form-group">
                                            <label for="attributeDataType">New Data Type:</label>
                                            <asp:DropDownList ID="ddlEditMultiValuedAttributeNewDataType" name="attributeDataType" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                                <asp:ListItem Text="-- Select Data Type --" />
                                                <asp:ListItem Text="Int" Value="Int" />
                                                <asp:ListItem Text="Nvarchar" Value="Nvarchar" />
                                                <asp:ListItem Text="Float" Value="Float" />
                                                <asp:ListItem Text="Bit" Value="Bit" />
                                                 <asp:ListItem Text="Bit" Value="Date" />
                                                <asp:ListItem Text="Bit" Value="Boolean" />
                                                <asp:ListItem Text="Bit" Value="Binary" />
                                                  <asp:ListItem Text="Bit" Value="Char" />
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group">
                                            <label for="attributeLength">New Attribute Length:</label>
                                            <asp:TextBox ID="txtEditMultiValuedAttributeNewLength" name="attributeLength" runat="server" CssClass="form-control" />
                                        </div>
                                        <div class="form-group">
                                            <asp:Button ID="Button17" Text="Modify" runat="server" CssClass="btn btn-default" OnClick="btnEditMultiValuedAttributeSave_Click" />
                                        </div>
                                    </div>
                                </div>
                                <div class="tab-pane" id="DeleteMultiValuedAttribute">
                                    <div class="form" role="form">
                                        <div class="form-group">
                                            <label for="attributeEntity">Select Attribute:</label>
                                            <asp:DropDownList ID="ddlDeleteMultiValuedAttribute" name="attributeEntity" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="-- Select Attribute --" />
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group">
                                            <asp:Button ID="Button26" Text="Delete" runat="server" CssClass="btn btn-default" onclick="btnDeleteMultiValuedAttribute_Click"/>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="Button18" Text="Cancel" runat="server" CssClass="btn btn-default" OnClick="cancel_Click" />
                    </div>
                </div>
            </div>
        </div>

        <div id="modalAddRelationship" class="modal fade" role="dialog">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Relationship</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form" role="form">
                            <div class="form-group">
                                <label for="relationshipName">Relationship Name:</label>
                                <asp:TextBox ID="txtRelationshipName" name="relationshipName" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <label for="primaryAttribute">Primary Table/ Attribute:</label>
                                <asp:DropDownList ID="ddlPrimaryAttribute" name="primaryAttribute" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="-- Select Attribute --" />
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="foriegnAttribute">Foriegn Table/ Attribute:</label>
                                <asp:DropDownList ID="ddlforiegnAttribute" name="foriegnAttribute" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="-- Select Attribute --" />
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="relationshipType">Relatioship Type:</label>
                                <asp:DropDownList ID="ddlRelationshipType" name="relationshipType" AppendDataBoundItems="true" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="-- Select Type --" />
                                    <asp:ListItem Text="1:1" />
                                    <asp:ListItem Text="1:N" />
                                    <asp:ListItem Text="N:1" />
                                    <asp:ListItem Text="N:N" />
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAddRelationship" Text="Insert" runat="server" CssClass="btn btn-default" OnClick="btnAddRelationship_Click" />
                        <asp:Button ID="Button7" Text="Cancel" runat="server" CssClass="btn btn-default" OnClick="cancel_Click" />
                    </div>
                </div>
            </div>
        </div>

        <div id="modalEditRelationship" class="modal fade" role="dialog">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Relationship</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form" role="form">
                            <div class="form-group">
                                <label for="editEntity">Select Relationship:</label>
                                <asp:DropDownList ID="ddlEditRelationshipSelect" name="editEntity" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="-- Select Relationship --" />
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="relationshipName">Relationship New Name:</label>
                                <asp:TextBox ID="txtEditRelationshipNewName" name="relationshipName" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <label for="primaryAttribute">Primary Table/ Attribute:</label>
                                <asp:DropDownList ID="ddlEditPrimaryAttribute" name="primaryAttribute" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="-- Select Attribute --" />
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="foriegnAttribute">Foriegn Table/ Attribute:</label>
                                <asp:DropDownList ID="ddlEditforiegnAttribute" name="foriegnAttribute" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="-- Select Attribute --" />
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="relationshipType">Relatioship Type:</label>
                                <asp:DropDownList ID="ddlEditRelationshipType" name="relationshipType" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="-- Select Type --" />
                                    <asp:ListItem Text="1:1" />
                                    <asp:ListItem Text="1:N" />
                                    <asp:ListItem Text="N:1" />
                                    <asp:ListItem Text="N:N" />
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="Button19" Text="Insert" runat="server" CssClass="btn btn-default" OnClick="btnEditRelationship_Click" />
                        <asp:Button ID="Button20" Text="Cancel" runat="server" CssClass="btn btn-default" OnClick="cancel_Click" />
                    </div>
                </div>
            </div>
        </div>

        <div id="modalAddWeakRelationship" class="modal fade" role="dialog">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Weak Relationship</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form" role="form">
                            <div class="form-group">
                                <label for="relationshipName">Relationship Name:</label>
                                <asp:TextBox ID="txtWeakRelationshipName" name="relationshipName" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <label for="primaryAttribute">Primary Table/ Attribute:</label>
                                <asp:DropDownList ID="ddlWeakPrimaryAttribute" name="primaryAttribute" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="-- Select Attribute --" />
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="foriegnAttribute">Foriegn Table/ Attribute:</label>
                                <asp:DropDownList ID="ddlWeakforiegnAttribute" name="foriegnAttribute" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="-- Select Attribute --" />
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="relationshipType">Relatioship Type:</label>
                                <asp:DropDownList ID="ddlWeakRelationshipType" name="relationshipType" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="-- Select Type --" />
                                    <asp:ListItem Text="1:1" />
                                    <asp:ListItem Text="1:N" />
                                    <asp:ListItem Text="N:1" />
                                    <asp:ListItem Text="N:N" />
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAddWeakRelationship" Text="Insert" runat="server" CssClass="btn btn-default" OnClick="btnAddWeakRelationship_Click" />
                        <asp:Button ID="Button28" Text="Cancel" runat="server" CssClass="btn btn-default" OnClick="cancel_Click" />
                    </div>
                </div>
            </div>
        </div>

        <div id="modalEditWeakRelationship" class="modal fade" role="dialog">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Relationship</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form" role="form">
                            <div class="form-group">
                                <label for="editEntity">Select Relationship:</label>
                                <asp:DropDownList ID="ddlEditWeakRelationshipSelect" name="editEntity" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlEditEntitySelect_SelectedIndexChanged">
                                    <asp:ListItem Text="-- Select Entity --" />
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="relationshipName">Relationship New Name:</label>
                                <asp:TextBox ID="txtEditWeakRelationshipNewName" name="relationshipName" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <label for="primaryAttribute">Primary Table/ Attribute:</label>
                                <asp:DropDownList ID="ddlEditWeakPrimaryAttribute" name="primaryAttribute" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="-- Select Attribute --" />
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="foriegnAttribute">Foriegn Table/ Attribute:</label>
                                <asp:DropDownList ID="ddlEditWeakforiegnAttribute" name="foriegnAttribute" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="-- Select Attribute --" />
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="relationshipType">Relatioship Type:</label>
                                <asp:DropDownList ID="ddlEditWeakRelationshipType" name="relationshipType" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="-- Select Type --" />
                                    <asp:ListItem Text="1:1" />
                                    <asp:ListItem Text="1:N" />
                                    <asp:ListItem Text="N:1" />
                                    <asp:ListItem Text="N:N" />
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="Button29" Text="Insert" runat="server" CssClass="btn btn-default" OnClick="btnEditWeakRelationship_Click" />
                        <asp:Button ID="Button30" Text="Cancel" runat="server" CssClass="btn btn-default" OnClick="cancel_Click" />
                    </div>
                </div>
            </div>
        </div>

        <div id="modalGenerateDatabase" class="modal fade" role="dialog">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Script Generation</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form" role="form">
                            <div class="form-group">
                                <label for="databaseName">Database Name:</label>
                                <asp:TextBox ID="txtDatabaseName" name="databaseName" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <label for="hostName">Host Name:</label>
                                <asp:TextBox ID="txtHostName" name="hostName" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <label for="dbUsername">Username:</label>
                                <asp:TextBox ID="txtUsername" name="dbUsername" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <label for="dbPassword">Password:</label>
                                <asp:TextBox ID="txtPassword" name="dbPassword" TextMode="Password" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <asp:Button ID="btnGenerateDatabase" Text="Generate Database" runat="server" CssClass="btn btn-default" OnClick="btnGenerateDatabase_Click" />
                        <asp:Button ID="Button8" Text="Cancel" runat="server" CssClass="btn btn-default" OnClick="cancel_Click" />
                    </div>
                </div>
            </div>
        </div>

        <div id="modalSaveDiagram" class="modal fade" role="dialog">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Save</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form" role="form">
                            <div class="form-group">
                                <label for="diagramName">Diagram Name:</label>
                                <asp:TextBox ID="txtDiagramName" name="diagramName" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <asp:Button ID="btnSaveDiagram" Text="Save Diagram" runat="server" CssClass="btn btn-default" OnClick="btnSaveDiagram_Click" />
                        <asp:Button ID="Button9" Text="Cancel" runat="server" CssClass="btn btn-default" OnClick="cancel_Click" />
                    </div>
                </div>
            </div>
        </div>

        <div id="modalOpenDiagram" class="modal fade" role="dialog">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Open</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form" role="form">
                            <div class="form-group">
                                <label for="diagramName">Diagram Name:</label>
                                <asp:DropDownList ID="ddlSavedDiagramName" name="diagramName" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="-- Select Attribute --" />
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <asp:Button ID="btnOpenDiagram" Text="Open" runat="server" CssClass="btn btn-default" OnClick="btnOpenDiagram_Click" />
                        <asp:Button ID="Button10" Text="Cancel" runat="server" CssClass="btn btn-default" OnClick="cancel_Click" />
                    </div>
                </div>
            </div>
        </div>

        <div id="modalQuery" class="modal fade" role="dialog">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Database Query</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form" role="form">
                            <div class="form-group">
                                <label for="databaseName">Database Name:</label>
                                <asp:TextBox ID="txtQueryDatabase" name="databaseName" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <label for="hostName">Host Name:</label>
                                <asp:TextBox ID="txtQueryHost" name="hostName" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <label for="dbUsername">Username:</label>
                                <asp:TextBox ID="txtQueryUsername" name="dbUsername" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <label for="dbPassword">Password:</label>
                                <asp:TextBox ID="txtQueryPassword" name="dbPassword" TextMode="Password" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <label for="diagramName">Query Type:</label>
                                <asp:DropDownList ID="ddlQueryType" name="diagramName" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="-- Select Type --" />
                                    <asp:ListItem Text="SELECT" Value="SELECT" />
                                    <asp:ListItem Text="INSERT" Value="INSERT" />
                                    <asp:ListItem Text="UPDATE" Value="UPDATE" />
                                    <asp:ListItem Text="DELETE" Value="DELETE" />
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="dbUsername">Query String:</label>
                                <asp:TextBox ID="txtQueryString" name="txtQueryString" TextMode="MultiLine" Rows="5" runat="server" CssClass="form-control" />
                            </div>

                        </div>
                    </div>

                    <div class="modal-footer">
                        <asp:Button ID="btnQueryExecute" Text="Execute" runat="server" CssClass="btn btn-default" OnClick="btnQueryExecute_Click" />
                        <asp:Button ID="Button27" Text="Cancel" runat="server" CssClass="btn btn-default" OnClick="cancel_Click" />
                    </div>
                </div>
            </div>
        </div>

        <div id="modalHelp" class="modal fade" role="dialog">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Help</h4>
                    </div>
                    <div class="modal-body">
                        <div class="tabbable">
                            <ul class="nav nav-tabs">
                                <li class="active"><a href="#tabEntity" data-toggle="tab">Entity</a></li>
                                <li><a href="#tabAttribute" data-toggle="tab">Attribute</a></li>
                                <li><a href="#tabRelationship" data-toggle="tab">Relationship</a></li>
                            </ul>
                            <div class="tab-content">
                                <div class="tab-pane active" id="tabEntity">
                                    <b>When naming your database tables, give consideration to other steps 
in the development process. Keep in mind you will most likely have to utilize the names you give your tables several times as part of other 
objects, for example, procedures, triggers or views may all contain 
references to the table name. You want to keep the name as simple 
and short as possible. Some systems enforce character limits on object names also.<br/>
1-Table names should be singular.<br/>
2-Do not give your table names prefixes like "tb" or "TBL_" as these are 
redundant and wordy.<br/>
3-For table names, underscores should not be used.<br/>
4-Avoid using abbreviations if possible.<br/></b>
                                </div>
                                <div class="tab-pane" id="tabAttribute">
                                    <b>When naming your columns, keep in mind that they are members of 
the table, so they do not need the any mention of the table name in the name. When writing a query against the table, you should be 
prefixing the field name with the table name or an alias anyway. Just like with naming tables, avoid using abbreviations, acronyms or special 
characters. Allcolumn names should use PascalCase to distinguish them from SQL keywords (camelCase).<br/>
1-Identity Primary Key Fields the name should simply be [tableName] + “Id“.<br/>
2-oreign key fields should have the exact same name as they do in the parent table
where the field is the primary.
3-f you have tables with composite keys it’s recommended 
that a seeded identity column is created to use as the primary key for 
the table.<br/>
4-Do not prefix your fields with "fld_" or "Col_" as it should be obvious in SQL 
statements which items are columns.<br/>
5-Bit fields should be given affirmative boolean names.
6-Field names should be no longer than 50 characters and all should strive for
less lengthy names if possible.<br/>
7-Field names should contain only letters and numbers.</b>
                                </div>
                                <div class="tab-pane" id="tabRelationship">
                                    Data 2
                                </div>
                            </div>

                        </div>
                    </div>
                    <div class="modal-footer">

                        <asp:Button ID="Button12" Text="OK" runat="server" CssClass="btn btn-default" OnClick="cancel_Click" />
                    </div>
                </div>
            </div>
        </div>

        <asp:HiddenField ID="xLoc" runat="server" />
        <asp:HiddenField ID="yLoc" runat="server" />
    </form>
</body>
</html>
