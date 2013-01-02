namespace Yaircc
{
    partial class ThemeManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Default");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Terminal");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Official Themes", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Custom Themes");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ThemeManager));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.themesTreeView = new System.Windows.Forms.TreeView();
            this.treeViewImageList = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.themePreviewBrowser = new System.Windows.Forms.WebBrowser();
            this.createdByLinkLabel = new System.Windows.Forms.LinkLabel();
            this.themeDescriptionLabel = new System.Windows.Forms.Label();
            this.themeNameLabel = new System.Windows.Forms.Label();
            this.closeButton = new System.Windows.Forms.Button();
            this.removeButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.defaultButton = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(6, 8);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.themesTreeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(973, 542);
            this.splitContainer1.SplitterDistance = 236;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.TabStop = false;
            // 
            // themesTreeView
            // 
            this.themesTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.themesTreeView.ImageIndex = 0;
            this.themesTreeView.ImageList = this.treeViewImageList;
            this.themesTreeView.Location = new System.Drawing.Point(0, 0);
            this.themesTreeView.Name = "themesTreeView";
            treeNode1.ImageKey = "layout";
            treeNode1.Name = "DefaultNode";
            treeNode1.SelectedImageKey = "layout";
            treeNode1.Text = "Default";
            treeNode2.ImageKey = "layout";
            treeNode2.Name = "TerminalNode";
            treeNode2.SelectedImageKey = "layout";
            treeNode2.Text = "Terminal";
            treeNode3.ImageKey = "folder";
            treeNode3.Name = "OfficialThemesNode";
            treeNode3.SelectedImageKey = "folder";
            treeNode3.Text = "Official Themes";
            treeNode4.ImageKey = "folder";
            treeNode4.Name = "CustomThemesNode";
            treeNode4.SelectedImageKey = "folder";
            treeNode4.Text = "Custom Themes";
            this.themesTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode4});
            this.themesTreeView.SelectedImageIndex = 0;
            this.themesTreeView.Size = new System.Drawing.Size(236, 542);
            this.themesTreeView.TabIndex = 0;
            this.themesTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ThemesTreeView_AfterSelect);
            this.themesTreeView.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.ThemesTreeView_BeforeSelect);
            // 
            // treeViewImageList
            // 
            this.treeViewImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("treeViewImageList.ImageStream")));
            this.treeViewImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.treeViewImageList.Images.SetKeyName(0, "folder");
            this.treeViewImageList.Images.SetKeyName(1, "layout");
            this.treeViewImageList.Images.SetKeyName(2, "tick");
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.createdByLinkLabel);
            this.panel1.Controls.Add(this.themeDescriptionLabel);
            this.panel1.Controls.Add(this.themeNameLabel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(733, 542);
            this.panel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.themePreviewBrowser);
            this.groupBox1.Location = new System.Drawing.Point(17, 62);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(695, 442);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Preview";
            // 
            // themePreviewBrowser
            // 
            this.themePreviewBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.themePreviewBrowser.IsWebBrowserContextMenuEnabled = false;
            this.themePreviewBrowser.Location = new System.Drawing.Point(3, 17);
            this.themePreviewBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.themePreviewBrowser.Name = "themePreviewBrowser";
            this.themePreviewBrowser.Size = new System.Drawing.Size(689, 422);
            this.themePreviewBrowser.TabIndex = 6;
            this.themePreviewBrowser.WebBrowserShortcutsEnabled = false;
            // 
            // createdByLinkLabel
            // 
            this.createdByLinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.createdByLinkLabel.LinkArea = new System.Windows.Forms.LinkArea(11, 9);
            this.createdByLinkLabel.Location = new System.Drawing.Point(20, 507);
            this.createdByLinkLabel.Name = "createdByLinkLabel";
            this.createdByLinkLabel.Size = new System.Drawing.Size(692, 18);
            this.createdByLinkLabel.TabIndex = 4;
            this.createdByLinkLabel.TabStop = true;
            this.createdByLinkLabel.Text = "Created by author";
            this.createdByLinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.createdByLinkLabel.UseCompatibleTextRendering = true;
            this.createdByLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.CreatedByLinkLabel_LinkClicked);
            // 
            // themeDescriptionLabel
            // 
            this.themeDescriptionLabel.Location = new System.Drawing.Point(14, 40);
            this.themeDescriptionLabel.Name = "themeDescriptionLabel";
            this.themeDescriptionLabel.Size = new System.Drawing.Size(466, 19);
            this.themeDescriptionLabel.TabIndex = 1;
            this.themeDescriptionLabel.Text = "Theme description.";
            // 
            // themeNameLabel
            // 
            this.themeNameLabel.AutoSize = true;
            this.themeNameLabel.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.themeNameLabel.Location = new System.Drawing.Point(12, 15);
            this.themeNameLabel.Name = "themeNameLabel";
            this.themeNameLabel.Size = new System.Drawing.Size(137, 25);
            this.themeNameLabel.TabIndex = 0;
            this.themeNameLabel.Text = "Theme Name";
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.Location = new System.Drawing.Point(904, 556);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 4;
            this.closeButton.Text = "&Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // removeButton
            // 
            this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.removeButton.Enabled = false;
            this.removeButton.Location = new System.Drawing.Point(87, 556);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(75, 23);
            this.removeButton.TabIndex = 2;
            this.removeButton.Text = "&Remove";
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addButton.Location = new System.Drawing.Point(6, 556);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 1;
            this.addButton.Text = "&Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // defaultButton
            // 
            this.defaultButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.defaultButton.Enabled = false;
            this.defaultButton.Location = new System.Drawing.Point(168, 556);
            this.defaultButton.Name = "defaultButton";
            this.defaultButton.Size = new System.Drawing.Size(75, 23);
            this.defaultButton.TabIndex = 3;
            this.defaultButton.Text = "&Default";
            this.defaultButton.UseVisualStyleBackColor = true;
            this.defaultButton.Click += new System.EventHandler(this.DefaultButton_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.AddExtension = false;
            this.openFileDialog.Filter = "CSS files|*.css";
            this.openFileDialog.Title = "Select a Theme";
            // 
            // ThemeManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(987, 588);
            this.Controls.Add(this.defaultButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ThemeManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Themes";
            this.Load += new System.EventHandler(this.ThemeManager_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.TreeView themesTreeView;
        private System.Windows.Forms.ImageList treeViewImageList;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label themeNameLabel;
        private System.Windows.Forms.Label themeDescriptionLabel;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button defaultButton;
        private System.Windows.Forms.LinkLabel createdByLinkLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.WebBrowser themePreviewBrowser;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
    }
}