using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;

namespace AD_test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void CheckButton_Click(object sender, EventArgs e)
        {
            //Name2TextBox.Text = GetObjectDistinguishedName(objectClass.user, returnType.distinguishedName, NameTextBox.Text, "nps.doi.net");
            Name2TextBox.Text = GetObjectDistinguishedName(objectClass.group , returnType.distinguishedName, NameTextBox.Text, "nps.doi.net");
        }

        private void GetDOmainsButton_Click(object sender, EventArgs e)
        {
            //ResultsGridView.DataSource = Users();
            ResultsListBox.DataSource = EnumerateDomains();
        }

        private static List<DirectoryEntry> Users()
        {
            List<DirectoryEntry> entries = new List<DirectoryEntry>();
            DirectoryEntry domain = new DirectoryEntry("LDAP://nps.doi.net/CN=Users,DC=nps,DC=doi,DC=net");
            foreach (DirectoryEntry child in domain.Children)
            {
                entries.Add(child);
            }
            return entries;
        }

        private static List<string> Users2()
        {
            List<string> entries = new List<string>();
            //No CN=User, No OU=Users, No OU=user, CN=Group,OU=GROUPS, CN=Groups
            DirectoryEntry domain = new DirectoryEntry("LDAP://nps.doi.net/OU=Users,DC=nps,DC=doi,DC=net");
            foreach (DirectoryEntry child in domain.Children)
            {
                entries.Add(child.Path.Remove(0, 7));
            }
            return entries;
        }
        private static List<string> Users3()
        {
            List<string> entries = new List<string>();
            DirectoryEntry domain = new DirectoryEntry("LDAP://OU=Users,OU=OU_AKRO,OU=AKR,DC=nps,DC=doi,DC=net");
            //DirectoryEntry domain = new DirectoryEntry("LDAP://OU=Users,OU=OU_AKRO,DC=nps,DC=doi,DC=net");
            //DirectoryEntry domain = new DirectoryEntry("LDAP://OU=Users,DC=nps,DC=doi,DC=net");
            foreach (DirectoryEntry child in domain.Children)
            {
                entries.Add(child.Path.Remove(0, 7));
            }
            return entries;
        }

        private static List<string> Groups(string dn)
        {
            DirectoryEntry domain = new DirectoryEntry(dn);
            return (from DirectoryEntry child in domain.Children orderby child.Path select child.Path.Remove(0, 7)).ToList();
        }

        public static string FriendlyDomainToLdapDomain(string friendlyDomainName)
        {
            string ldapPath = null;
            try
            {
                DirectoryContext objContext = new DirectoryContext(
                    DirectoryContextType.Domain, friendlyDomainName);
                Domain objDomain = Domain.GetDomain(objContext);
                ldapPath = objDomain.Name;
            }
            catch (DirectoryServicesCOMException e)
            {
                ldapPath = e.Message.ToString();
            }
            return ldapPath;
        }

        public static List<string> EnumerateDomains()
        {
            Forest currentForest = Forest.GetCurrentForest();
            DomainCollection myDomains = currentForest.Domains;
            return (from Domain objDomain in myDomains orderby objDomain.Name select objDomain.Name).ToList();
        }

        public static List<string> EnumerateGlobalCatalogs()
        {
            Forest currentForest = Forest.GetCurrentForest();
            return (from GlobalCatalog gc in currentForest.GlobalCatalogs orderby gc.Name select gc.Name).ToList();
        }

        public static List<string> EnumerateDomainControllers()
        {
            Domain domain = Domain.GetCurrentDomain();
            return (from DomainController dc in domain.DomainControllers orderby dc.Name select dc.Name).ToList();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            NameTextBox.Text = FriendlyDomainToLdapDomain("nps");
            ResultsListBox.DataSource = EnumerateDomains();
        }

        private void GetCatalogsButton_Click(object sender, EventArgs e)
        {
            ResultsListBox.DataSource = EnumerateGlobalCatalogs();
        }

        private void GetDCButton_Click(object sender, EventArgs e)
        {
            ResultsListBox.DataSource = EnumerateDomainControllers();
        }

        private void GetGroupsButton_Click(object sender, EventArgs e)
        {
            //ResultsListBox.DataSource = Groups(Name2TextBox.Text);
            GetGroups();
        }

        private void GetUsersButton_Click(object sender, EventArgs e)
        {
            //ResultsListBox.DataSource = Users3();
            GetUsers();
        }

        public string GetObjectDistinguishedName(objectClass objectCls, returnType returnValue, string objectName, string ldapDomain)
        {
            string distinguishedName = string.Empty;
            string connectionPrefix = "LDAP://" + ldapDomain;
            DirectoryEntry entry = new DirectoryEntry(connectionPrefix);
            DirectorySearcher mySearcher = new DirectorySearcher(entry);

            switch (objectCls)
            {
                case objectClass.user:
                    mySearcher.Filter = "(&(objectClass=user) (|(cn=" + objectName + ")(sAMAccountName=" + objectName + ")))";
                    break;
                case objectClass.group:
                    mySearcher.Filter = "(&(objectClass=group) (|(cn=" + objectName + ")(dn=" + objectName + ")))";
                    break;
                case objectClass.computer:
                    mySearcher.Filter = "(&(objectClass=computer) (|(cn=" + objectName + ")(dn=" + objectName + ")))";
                    break;
            }
            SearchResult result = mySearcher.FindOne();

            if (result == null)
            {
                string msg = "unable to locate the distinguishedName for the object " +
                objectName + " in the " + ldapDomain + " domain";
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }
            DirectoryEntry directoryObject = result.GetDirectoryEntry();
            if (returnValue.Equals(returnType.distinguishedName))
            {
                distinguishedName = "LDAP://" + directoryObject.Properties
                    ["distinguishedName"].Value;
            }
            if (returnValue.Equals(returnType.ObjectGUID))
            {
                distinguishedName = directoryObject.Guid.ToString();
            }
            entry.Close();
            entry.Dispose();
            mySearcher.Dispose();
            return distinguishedName;
        }

        void GetUsers()
        {
            var context = new PrincipalContext(ContextType.Domain, "nps", "OU=AKR,DC=nps,DC=doi,DC=net");
            //UserPrincipal userPrincipal =  UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, "resarwas");
            var data = ListUsers(context, NameTextBox.Text + "*" ).ToList();
            ResultsGridView.DataSource = data;
            var query = from Principal p in data orderby p.DisplayName select p.Name + "(" + p.Description + ") - NPS\\" + p.SamAccountName;
            ResultsListBox.DataSource = query.ToList();
        }

        void GetGroups()
        {
            var context = new PrincipalContext(ContextType.Domain, "nps", "DC=nps,DC=doi,DC=net");
            var data = ListGroups(context, NameTextBox.Text).ToList();
            ResultsGridView.DataSource = data;
            var query = from Principal p in data orderby p.SamAccountName select p.SamAccountName;
            ResultsListBox.DataSource = query.ToList();
        }

        private PrincipalSearchResult<Principal> ListGroups(PrincipalContext context, string search)
        {
            var principal = new GroupPrincipal(context) { Name = search };
            return Search(principal);
        }
        private PrincipalSearchResult<Principal> ListUsers(PrincipalContext context, string search)
        {
            var principal = new UserPrincipal(context) { Surname = search, Enabled = true };
            return Search(principal);
        }

        private static PrincipalSearchResult<Principal> Search(Principal user)
        {
            var searcher = new PrincipalSearcher { QueryFilter = user };
            return searcher.FindAll();
        }

    }

    public enum objectClass
    {
        user, group, computer
    }
    public enum returnType
    {
        distinguishedName, ObjectGUID
    }
}
