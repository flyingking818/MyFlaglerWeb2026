<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MainForm.aspx.cs" Inherits="MyFlaglerWeb2026.PersonApp.MainForm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Personnel Management</h1>
    <p>
        <asp:Panel ID="PanelForm" runat="server">
        <table>
            <tr>
                <td style="width: 200px">Person Type</td>
                <td style="width: 800px">
                    <asp:RadioButtonList ID="rblPersonType" runat="server" RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="rblPersonType_SelectedIndexChanged">
                        <asp:ListItem>Professor</asp:ListItem>
                        <asp:ListItem>Student</asp:ListItem>
                        <asp:ListItem>Staff</asp:ListItem>
                    </asp:RadioButtonList>

                </td>
            </tr>
            <tr>
                <td>Basic Information</td>
                <td>
                    <table>
                        <tr>
                            <td style="width: 150px;">Name*: </td>
                            <td style="width: 400px;">
                                <asp:TextBox runat="server" ID="txtName"></asp:TextBox><asp:RequiredFieldValidator runat="server" ErrorMessage="Please enter your name!" ControlToValidate="txtName" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>ID: </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtID"></asp:TextBox><asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="Please enter a valid ID number!" ControlToValidate="txtID" MinimumValue="100" MaximumValue="1000" Type="Integer" ForeColor="red"></asp:RangeValidator> 
                            </td>
                        </tr>
                        <tr>
                            <td>Email:</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtEmail"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
           
            <asp:Panel ID="pnlProfessor" runat="server">
                <tr>
                    <td>Professor Information</td>
                    <td>
                        <table>
                            <tr>
                                <td style="width: 150px;">Department:</td>
                                <td style="width: 400px;">
                                    <asp:DropDownList ID="ddlDepartment" runat="server">
                                        <asp:ListItem>---Please Select---</asp:ListItem>
                                        <asp:ListItem Value="MAT">Math and Technology</asp:ListItem>
                                        <asp:ListItem Value="BA">Business Administration</asp:ListItem>
                                        <asp:ListItem Value="Eng">English</asp:ListItem>
                                        <asp:ListItem Value="Psy">Psychology</asp:ListItem>
                                        <asp:ListItem>Other</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Research Area: </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtResearchArea"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:CheckBox ID="chkTerminalDegree" Text="Terminal Degree?" runat="server" />
                                </td>
                            </tr>
                        </table>

                    </td>
                </tr>
            </asp:Panel>

            <asp:Panel ID="pnlStudent" runat="server">
                <tr>
                    <td>Student Information</td>
                    <td>
                        <table>
                            <tr>
                                <td style="width: 150px;">Major:</td>
                                <td style="width: 400px;">
                                    <asp:TextBox runat="server" ID="txtMajor"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>GPA: </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtGPA"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Enrollment Date: </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtEnrollmentDate"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Is Full Time?</td>
                                <td>
                                    <asp:CheckBox ID="chkFullTime" Text="Full time?" runat="server" />
                                </td>
                            </tr>
                        </table>

                    </td>
                </tr>
            </asp:Panel>

            <asp:Panel ID="pnlStaff" runat="server">
                <tr>
                    <td>Staff Information</td>
                    <td>
                        <table>
                            <tr>
                                <td style="width: 150px;">Position:</td>
                                <td style="width: 400px;">
                                    <asp:TextBox runat="server" ID="txtPosition"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Division: </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtDivision"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:CheckBox ID="chkAdministrative" Text="Is Administrative?" runat="server" />
                                </td>
                            </tr>
                        </table>

                    </td>
                </tr>
            </asp:Panel>
          

            <tr>
                <td><asp:Button ID="btnDisplayProfile" runat="server" Text="Display Profile" OnClick="btnDisplayProfile_Click"></asp:Button></td>
                <td>
                    

                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnAddProfile" runat="server" Text="Add Profile" OnClick="btnAddProfile_Click"></asp:Button></td>
                <td>&nbsp;</td>
            </tr>
        </table>
        </asp:Panel>
    </p>
    <p>
        <asp:Label ID="lblResult" runat="server" Text=""></asp:Label>

    </p>



</asp:Content>