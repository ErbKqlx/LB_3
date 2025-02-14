using LB_3.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace LB_3
{
    public partial class FormMain : Form
    {
        private BlogContext? db;

        public FormMain()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.db = new BlogContext();

            this.db.Users.Load();
            this.dataGridViewUsers.DataSource = db.Users.Local.ToBindingList();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            this.db?.Dispose();
            this.db = null;
        }

        private void DataGridViewUsers_SelectionChanged(object sender, EventArgs e)
        {
            if (this.db != null)
            {
                if (this.dataGridViewUsers.CurrentRow != null)
                {
                    var user = (User)this.dataGridViewUsers.CurrentRow.DataBoundItem;

                    if (user != null)
                    {
                        this.db.Entry(user).Collection(e => e.Posts).Load();
                        this.dataGridViewPosts.DataSource = user.Posts;
                    }
                }
            }
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            this.db!.SaveChanges();

            this.dataGridViewUsers.Refresh();
            this.dataGridViewPosts.Refresh();
        }
    }
}
