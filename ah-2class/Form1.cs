using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace ah_2class
{
    
    public partial class FrmMain : Form
    {
        public delegate void RefreshInfo(User user);
        public List<User> Users=new List<User> { };
        public void RefreshList(User user)
        {
            //Console.WriteLine(user.ToString());
            this.Invoke(new Action(() =>
            {
                pbComplation.PerformStep();
                txtStatu.Text = user.ToString();
                if (pbComplation.Value==pbComplation.Maximum)
                {
                    btnStart.Enabled = true;
                    btnCleanList.Enabled = true;
                    btnImport.Enabled = true;
                    btnCleanComplate.Enabled = true;
                    txtStatu.Text = "已完成";
                }
                this.Users.Where(r => r.UserName.Equals(user.UserName)).ToList()[0] = user;
                dgvList.DataSource = null;
                dgvList.AutoGenerateColumns = false;
                dgvList.DataSource = this.Users;

            }));



        }

        public FrmMain()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.Users.Count>0)
            {
                btnStart.Enabled = false;
                btnCleanList.Enabled = false;
                btnImport.Enabled = false;
                btnCleanComplate.Enabled = false;
                Service service = new Service();
                service.refresh += new RefreshInfo(RefreshList);
                service.Users = this.Users;
                pbComplation.Maximum = this.Users.Count;
                Thread thread = new Thread(new ThreadStart(service.Start));//创建线程

                thread.Start();
            }
            
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUserName.Clear();
            txtPassword.Clear();
            txtExceptPoint.Clear();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtUserName.Text.Length > 0)
            {
                User u = new User(txtUserName.Text);
                if (txtPassword.Text.Length > 0)
                {
                    u.Password = txtPassword.Text;
                }
                if (txtExceptPoint.Text.Length > 0)
                {
                    int p = Convert.ToInt32(txtExceptPoint.Text);
                    if (p>0)
                    {
                        u.ExpectPoint = p;
                    }
                }
                this.Users.Add(u);
                txtUserName.Clear();
                txtPassword.Clear();
                txtExceptPoint.Clear();
                dgvList.DataSource = null;
                dgvList.AutoGenerateColumns = false;
                dgvList.DataSource = this.Users;

            }
            
        }

        private void txtExceptPoint_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b' && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void dgvList_SelectionChanged(object sender, EventArgs e)
        {
            dgvList.ClearSelection();
        }

        private void dgvList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex==clExceptPoint.Index)
            {
                    if (e.Value.Equals(-1))
                    {
                        e.Value = "随机";
                    }
            }
        }

        private void btnCleanList_Click(object sender, EventArgs e)
        {
            this.Users.Clear();
            dgvList.DataSource = null;

        }

        private void btnCleanComplate_Click(object sender, EventArgs e)
        {
            this.Users.RemoveAll(r => r.Point != null);
            dgvList.DataSource = null;
            dgvList.AutoGenerateColumns = false;
            dgvList.DataSource = this.Users;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog()==DialogResult.OK)
            {
                try
                {
                    this.Users = Service.GetUsers(ofd.FileName);
                    dgvList.DataSource = null;
                    dgvList.AutoGenerateColumns = false;
                    dgvList.DataSource = this.Users;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "导入错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
