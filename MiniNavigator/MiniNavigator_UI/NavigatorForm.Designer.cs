using Infralution.Controls.VirtualTree;

namespace MiniNavigator_UI
{
    partial class NavigatorForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.colMain = new Infralution.Controls.VirtualTree.Column();
            this.NavigatorSplitContainer = new System.Windows.Forms.SplitContainer();
            this.NavigatorVirtualTree = new Infralution.Controls.VirtualTree.VirtualTree();
            this.NavigatorDataGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.NavigatorSplitContainer)).BeginInit();
            this.NavigatorSplitContainer.Panel1.SuspendLayout();
            this.NavigatorSplitContainer.Panel2.SuspendLayout();
            this.NavigatorSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NavigatorVirtualTree)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NavigatorDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // colMain
            // 
            this.colMain.AutoSizePolicy = Infralution.Controls.VirtualTree.ColumnAutoSizePolicy.AutoIncrease;
            this.colMain.Caption = "Объекты";
            this.colMain.DataField = "__MAIN__";
            this.colMain.Hidable = false;
            this.colMain.Movable = false;
            this.colMain.Name = "colMain";
            this.colMain.Width = 362;
            // 
            // NavigatorSplitContainer
            // 
            this.NavigatorSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NavigatorSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.NavigatorSplitContainer.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.NavigatorSplitContainer.Name = "NavigatorSplitContainer";
            NavigatorSplitContainer.SplitterMoved += NavigatorSplitContainer_SplitterMoved;
            // 
            // NavigatorSplitContainer.Panel1
            // 
            this.NavigatorSplitContainer.Panel1.Controls.Add(this.NavigatorVirtualTree);
            // 
            // NavigatorSplitContainer.Panel2
            // 
            this.NavigatorSplitContainer.Panel2.Controls.Add(this.NavigatorDataGridView);
            this.NavigatorSplitContainer.Size = new System.Drawing.Size(1178, 705);
            this.NavigatorSplitContainer.SplitterDistance = 382;
            this.NavigatorSplitContainer.SplitterWidth = 6;
            this.NavigatorSplitContainer.TabIndex = 1;
            // 
            // NavigatorVirtualTree
            // 
            this.NavigatorVirtualTree.Columns.Add(this.colMain);
            this.NavigatorVirtualTree.ConnectionOffset = 12;
            this.NavigatorVirtualTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NavigatorVirtualTree.HeaderHeight = 27;
            this.NavigatorVirtualTree.IndentWidth = 30;
            this.NavigatorVirtualTree.Location = new System.Drawing.Point(0, 0);
            this.NavigatorVirtualTree.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.NavigatorVirtualTree.MaxRowHeight = 184;
            this.NavigatorVirtualTree.MinRowHeight = 21;
            this.NavigatorVirtualTree.Name = "NavigatorVirtualTree";
            this.NavigatorVirtualTree.RowHeaderWidth = 45;
            this.NavigatorVirtualTree.RowHeight = 27;
            this.NavigatorVirtualTree.ScalingFactor = 1.5F;
            this.NavigatorVirtualTree.Size = new System.Drawing.Size(382, 705);
            this.NavigatorVirtualTree.TabIndex = 0;
            this.NavigatorVirtualTree.MouseUp += NavigatorVirtualTree_MouseUp;
            // 
            // NavigatorDataGridView
            // 
            this.NavigatorDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.NavigatorDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NavigatorDataGridView.Location = new System.Drawing.Point(0, 0);
            this.NavigatorDataGridView.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.NavigatorDataGridView.Name = "NavigatorDataGridView";
            this.NavigatorDataGridView.RowHeadersWidth = 62;
            this.NavigatorDataGridView.Size = new System.Drawing.Size(790, 705);
            this.NavigatorDataGridView.TabIndex = 0;
            // 
            // NavigatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1178, 705);
            this.Controls.Add(this.NavigatorSplitContainer);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "NavigatorForm";
            this.Text = "MiniNavigator";
            this.NavigatorSplitContainer.Panel1.ResumeLayout(false);
            this.NavigatorSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NavigatorSplitContainer)).EndInit();
            this.NavigatorSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NavigatorVirtualTree)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NavigatorDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        private void NavigatorSplitContainer_SplitterMoved(object sender, System.Windows.Forms.SplitterEventArgs e)
        {
            this.colMain.Width = this.NavigatorSplitContainer.Panel1.ClientSize.Width - 2;
        }

        #endregion

        private Infralution.Controls.VirtualTree.VirtualTree NavigatorVirtualTree;
        private Infralution.Controls.VirtualTree.Column colMain;
        private System.Windows.Forms.SplitContainer NavigatorSplitContainer;
        private System.Windows.Forms.DataGridView NavigatorDataGridView;
    }
}