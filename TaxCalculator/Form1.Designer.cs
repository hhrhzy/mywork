namespace TaxCalculator
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.textBox_jishu = new System.Windows.Forms.TextBox();
            this.textBox_GeRenBiLi = new System.Windows.Forms.TextBox();
            this.radioButton_sz = new System.Windows.Forms.RadioButton();
            this.radioButton_dg = new System.Windows.Forms.RadioButton();
            this.label_jishu = new System.Windows.Forms.Label();
            this.label_GeRenBiLi = new System.Windows.Forms.Label();
            this.button_MianShuiGJJ = new System.Windows.Forms.Button();
            this.resultBox_MianShui = new System.Windows.Forms.TextBox();
            this.label_GJJresult = new System.Windows.Forms.Label();
            this.textBox_shebao = new System.Windows.Forms.TextBox();
            this.textBox_Yingfa = new System.Windows.Forms.TextBox();
            this.textBox_mgjj = new System.Windows.Forms.TextBox();
            this.label_YingNaShui = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.resultBox_GeRenSuoDe = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.button_ShuiCha = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button_YingNaShui = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_msImp = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button_imGs = new System.Windows.Forms.Button();
            this.radioButton_forg = new System.Windows.Forms.RadioButton();
            this.radioButton_zg = new System.Windows.Forms.RadioButton();
            this.label_statu = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox_jishu
            // 
            this.textBox_jishu.Location = new System.Drawing.Point(71, 60);
            this.textBox_jishu.Margin = new System.Windows.Forms.Padding(5);
            this.textBox_jishu.Name = "textBox_jishu";
            this.textBox_jishu.Size = new System.Drawing.Size(69, 29);
            this.textBox_jishu.TabIndex = 0;
            this.textBox_jishu.TextChanged += new System.EventHandler(this.textBox_jishu_TextChanged);
            // 
            // textBox_GeRenBiLi
            // 
            this.textBox_GeRenBiLi.Location = new System.Drawing.Point(277, 60);
            this.textBox_GeRenBiLi.Margin = new System.Windows.Forms.Padding(5);
            this.textBox_GeRenBiLi.Name = "textBox_GeRenBiLi";
            this.textBox_GeRenBiLi.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.textBox_GeRenBiLi.Size = new System.Drawing.Size(59, 29);
            this.textBox_GeRenBiLi.TabIndex = 1;
            this.textBox_GeRenBiLi.TextChanged += new System.EventHandler(this.textBox_GeRenBiLi_TextChanged);
            // 
            // radioButton_sz
            // 
            this.radioButton_sz.AutoSize = true;
            this.radioButton_sz.Location = new System.Drawing.Point(162, 22);
            this.radioButton_sz.Margin = new System.Windows.Forms.Padding(5);
            this.radioButton_sz.Name = "radioButton_sz";
            this.radioButton_sz.Size = new System.Drawing.Size(60, 25);
            this.radioButton_sz.TabIndex = 2;
            this.radioButton_sz.Text = "深圳";
            this.radioButton_sz.UseVisualStyleBackColor = true;
            this.radioButton_sz.CheckedChanged += new System.EventHandler(this.radioButton_sz_CheckedChanged);
            // 
            // radioButton_dg
            // 
            this.radioButton_dg.AutoSize = true;
            this.radioButton_dg.Checked = true;
            this.radioButton_dg.Location = new System.Drawing.Point(92, 22);
            this.radioButton_dg.Margin = new System.Windows.Forms.Padding(5);
            this.radioButton_dg.Name = "radioButton_dg";
            this.radioButton_dg.Size = new System.Drawing.Size(60, 25);
            this.radioButton_dg.TabIndex = 3;
            this.radioButton_dg.TabStop = true;
            this.radioButton_dg.Text = "东莞";
            this.radioButton_dg.UseVisualStyleBackColor = true;
            this.radioButton_dg.CheckedChanged += new System.EventHandler(this.radioButton_dg_CheckedChanged);
            // 
            // label_jishu
            // 
            this.label_jishu.AutoSize = true;
            this.label_jishu.Location = new System.Drawing.Point(13, 65);
            this.label_jishu.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label_jishu.Name = "label_jishu";
            this.label_jishu.Size = new System.Drawing.Size(58, 21);
            this.label_jishu.TabIndex = 4;
            this.label_jishu.Text = "基数：";
            // 
            // label_GeRenBiLi
            // 
            this.label_GeRenBiLi.AutoSize = true;
            this.label_GeRenBiLi.Location = new System.Drawing.Point(164, 65);
            this.label_GeRenBiLi.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label_GeRenBiLi.Name = "label_GeRenBiLi";
            this.label_GeRenBiLi.Size = new System.Drawing.Size(90, 21);
            this.label_GeRenBiLi.TabIndex = 5;
            this.label_GeRenBiLi.Text = "个人比例：";
            // 
            // button_MianShuiGJJ
            // 
            this.button_MianShuiGJJ.Location = new System.Drawing.Point(380, 53);
            this.button_MianShuiGJJ.Margin = new System.Windows.Forms.Padding(5);
            this.button_MianShuiGJJ.Name = "button_MianShuiGJJ";
            this.button_MianShuiGJJ.Size = new System.Drawing.Size(74, 40);
            this.button_MianShuiGJJ.TabIndex = 6;
            this.button_MianShuiGJJ.Text = "计算";
            this.button_MianShuiGJJ.UseVisualStyleBackColor = true;
            this.button_MianShuiGJJ.Click += new System.EventHandler(this.button_MianShuiGJJ_Click);
            // 
            // resultBox_MianShui
            // 
            this.resultBox_MianShui.Location = new System.Drawing.Point(629, 61);
            this.resultBox_MianShui.Margin = new System.Windows.Forms.Padding(5);
            this.resultBox_MianShui.Name = "resultBox_MianShui";
            this.resultBox_MianShui.ReadOnly = true;
            this.resultBox_MianShui.Size = new System.Drawing.Size(100, 29);
            this.resultBox_MianShui.TabIndex = 7;
            this.resultBox_MianShui.MouseClick += new System.Windows.Forms.MouseEventHandler(this.resultBox_MianShui_MouseClick);
            // 
            // label_GJJresult
            // 
            this.label_GJJresult.AutoSize = true;
            this.label_GJJresult.Location = new System.Drawing.Point(490, 63);
            this.label_GJJresult.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label_GJJresult.Name = "label_GJJresult";
            this.label_GJJresult.Size = new System.Drawing.Size(138, 21);
            this.label_GJJresult.TabIndex = 8;
            this.label_GJJresult.Text = "东莞免税公积金：";
            // 
            // textBox_shebao
            // 
            this.textBox_shebao.Location = new System.Drawing.Point(125, 114);
            this.textBox_shebao.Margin = new System.Windows.Forms.Padding(5);
            this.textBox_shebao.Name = "textBox_shebao";
            this.textBox_shebao.Size = new System.Drawing.Size(81, 29);
            this.textBox_shebao.TabIndex = 10;
            this.textBox_shebao.TextChanged += new System.EventHandler(this.textBox_shebao_TextChanged);
            // 
            // textBox_Yingfa
            // 
            this.textBox_Yingfa.Location = new System.Drawing.Point(125, 72);
            this.textBox_Yingfa.Margin = new System.Windows.Forms.Padding(5);
            this.textBox_Yingfa.Name = "textBox_Yingfa";
            this.textBox_Yingfa.Size = new System.Drawing.Size(81, 29);
            this.textBox_Yingfa.TabIndex = 9;
            this.textBox_Yingfa.TextChanged += new System.EventHandler(this.textBox_Yingfa_TextChanged);
            // 
            // textBox_mgjj
            // 
            this.textBox_mgjj.Location = new System.Drawing.Point(125, 153);
            this.textBox_mgjj.Margin = new System.Windows.Forms.Padding(5);
            this.textBox_mgjj.Name = "textBox_mgjj";
            this.textBox_mgjj.Size = new System.Drawing.Size(81, 29);
            this.textBox_mgjj.TabIndex = 11;
            this.textBox_mgjj.TextChanged += new System.EventHandler(this.textBox_mgjj_TextChanged);
            // 
            // label_YingNaShui
            // 
            this.label_YingNaShui.AutoSize = true;
            this.label_YingNaShui.Location = new System.Drawing.Point(25, 74);
            this.label_YingNaShui.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label_YingNaShui.Name = "label_YingNaShui";
            this.label_YingNaShui.Size = new System.Drawing.Size(90, 21);
            this.label_YingNaShui.TabIndex = 12;
            this.label_YingNaShui.Text = "应发合计：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 117);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 21);
            this.label5.TabIndex = 13;
            this.label5.Text = "   扣社保：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 156);
            this.label6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(106, 21);
            this.label6.TabIndex = 14;
            this.label6.Text = "免税公积金：";
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(559, 280);
            this.textBox7.Margin = new System.Windows.Forms.Padding(5);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(106, 29);
            this.textBox7.TabIndex = 15;
            this.textBox7.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox7_KeyPress);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(559, 314);
            this.label7.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(106, 21);
            this.label7.TabIndex = 16;
            this.label7.Text = "补税后金额：";
            // 
            // resultBox_GeRenSuoDe
            // 
            this.resultBox_GeRenSuoDe.Location = new System.Drawing.Point(125, 197);
            this.resultBox_GeRenSuoDe.Margin = new System.Windows.Forms.Padding(5);
            this.resultBox_GeRenSuoDe.Name = "resultBox_GeRenSuoDe";
            this.resultBox_GeRenSuoDe.ReadOnly = true;
            this.resultBox_GeRenSuoDe.Size = new System.Drawing.Size(81, 29);
            this.resultBox_GeRenSuoDe.TabIndex = 18;
            this.resultBox_GeRenSuoDe.MouseClick += new System.Windows.Forms.MouseEventHandler(this.resultBox_GeRenSuoDe_MouseClick);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(587, 146);
            this.label9.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(58, 21);
            this.label9.TabIndex = 21;
            this.label9.Text = "税差：";
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(578, 340);
            this.textBox9.Margin = new System.Windows.Forms.Padding(5);
            this.textBox9.Name = "textBox9";
            this.textBox9.ReadOnly = true;
            this.textBox9.Size = new System.Drawing.Size(87, 29);
            this.textBox9.TabIndex = 20;
            // 
            // button_ShuiCha
            // 
            this.button_ShuiCha.Location = new System.Drawing.Point(559, 230);
            this.button_ShuiCha.Margin = new System.Windows.Forms.Padding(5);
            this.button_ShuiCha.Name = "button_ShuiCha";
            this.button_ShuiCha.Size = new System.Drawing.Size(100, 40);
            this.button_ShuiCha.TabIndex = 22;
            this.button_ShuiCha.Text = "计算";
            this.button_ShuiCha.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(346, 65);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 21);
            this.label1.TabIndex = 23;
            this.label1.Text = "%";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(250, 25);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label2.Size = new System.Drawing.Size(43, 17);
            this.label2.TabIndex = 24;
            this.label2.Text = "label2";
            // 
            // button_YingNaShui
            // 
            this.button_YingNaShui.Location = new System.Drawing.Point(14, 192);
            this.button_YingNaShui.Margin = new System.Windows.Forms.Padding(5);
            this.button_YingNaShui.Name = "button_YingNaShui";
            this.button_YingNaShui.Size = new System.Drawing.Size(103, 36);
            this.button_YingNaShui.TabIndex = 25;
            this.button_YingNaShui.Text = "计算个税：";
            this.button_YingNaShui.UseVisualStyleBackColor = true;
            this.button_YingNaShui.Click += new System.EventHandler(this.button_YingNaShui_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.button_msImp);
            this.groupBox1.Controls.Add(this.textBox_GeRenBiLi);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox_jishu);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.radioButton_sz);
            this.groupBox1.Controls.Add(this.radioButton_dg);
            this.groupBox1.Controls.Add(this.label_jishu);
            this.groupBox1.Controls.Add(this.label_GeRenBiLi);
            this.groupBox1.Controls.Add(this.button_MianShuiGJJ);
            this.groupBox1.Controls.Add(this.resultBox_MianShui);
            this.groupBox1.Controls.Add(this.label_GJJresult);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(736, 101);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "免税公积金";
            // 
            // button_msImp
            // 
            this.button_msImp.Location = new System.Drawing.Point(634, 16);
            this.button_msImp.Name = "button_msImp";
            this.button_msImp.Size = new System.Drawing.Size(95, 31);
            this.button_msImp.TabIndex = 25;
            this.button_msImp.Text = "Excel导入";
            this.button_msImp.UseVisualStyleBackColor = true;
            this.button_msImp.Click += new System.EventHandler(this.button_msImp_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.button_imGs);
            this.groupBox2.Controls.Add(this.radioButton_forg);
            this.groupBox2.Controls.Add(this.radioButton_zg);
            this.groupBox2.Controls.Add(this.button_YingNaShui);
            this.groupBox2.Controls.Add(this.resultBox_GeRenSuoDe);
            this.groupBox2.Controls.Add(this.textBox_Yingfa);
            this.groupBox2.Controls.Add(this.textBox_shebao);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label_YingNaShui);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.textBox_mgjj);
            this.groupBox2.Location = new System.Drawing.Point(12, 125);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(234, 272);
            this.groupBox2.TabIndex = 27;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "个税计算";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(95, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 17);
            this.label3.TabIndex = 29;
            this.label3.Text = "中国：3500|外籍4800";
            // 
            // button_imGs
            // 
            this.button_imGs.Location = new System.Drawing.Point(118, 234);
            this.button_imGs.Name = "button_imGs";
            this.button_imGs.Size = new System.Drawing.Size(88, 29);
            this.button_imGs.TabIndex = 28;
            this.button_imGs.Text = "Excel导入";
            this.button_imGs.UseVisualStyleBackColor = true;
            this.button_imGs.Click += new System.EventHandler(this.button_imGs_Click);
            // 
            // radioButton_forg
            // 
            this.radioButton_forg.AutoSize = true;
            this.radioButton_forg.Location = new System.Drawing.Point(109, 37);
            this.radioButton_forg.Name = "radioButton_forg";
            this.radioButton_forg.Size = new System.Drawing.Size(60, 25);
            this.radioButton_forg.TabIndex = 27;
            this.radioButton_forg.TabStop = true;
            this.radioButton_forg.Text = "外籍";
            this.radioButton_forg.UseVisualStyleBackColor = true;
            // 
            // radioButton_zg
            // 
            this.radioButton_zg.AutoSize = true;
            this.radioButton_zg.Checked = true;
            this.radioButton_zg.Location = new System.Drawing.Point(43, 37);
            this.radioButton_zg.Name = "radioButton_zg";
            this.radioButton_zg.Size = new System.Drawing.Size(60, 25);
            this.radioButton_zg.TabIndex = 26;
            this.radioButton_zg.TabStop = true;
            this.radioButton_zg.Text = "中国";
            this.radioButton_zg.UseVisualStyleBackColor = true;
            // 
            // label_statu
            // 
            this.label_statu.AutoSize = true;
            this.label_statu.BackColor = System.Drawing.Color.Transparent;
            this.label_statu.ForeColor = System.Drawing.Color.Red;
            this.label_statu.Location = new System.Drawing.Point(246, 406);
            this.label_statu.Name = "label_statu";
            this.label_statu.Size = new System.Drawing.Size(0, 21);
            this.label_statu.TabIndex = 26;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox3.Location = new System.Drawing.Point(261, 125);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(487, 272);
            this.groupBox3.TabIndex = 28;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "更多功能开发中";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(530, 394);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 26;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(763, 429);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label_statu);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.textBox9);
            this.Controls.Add(this.button_ShuiCha);
            this.Controls.Add(this.label9);
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TaxCalculator3.21   hzy_for_twj";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_jishu;
        private System.Windows.Forms.TextBox textBox_GeRenBiLi;
        private System.Windows.Forms.RadioButton radioButton_sz;
        private System.Windows.Forms.RadioButton radioButton_dg;
        private System.Windows.Forms.Label label_jishu;
        private System.Windows.Forms.Label label_GeRenBiLi;
        private System.Windows.Forms.Button button_MianShuiGJJ;
        private System.Windows.Forms.TextBox resultBox_MianShui;
        private System.Windows.Forms.Label label_GJJresult;
        private System.Windows.Forms.TextBox textBox_shebao;
        private System.Windows.Forms.TextBox textBox_Yingfa;
        private System.Windows.Forms.TextBox textBox_mgjj;
        private System.Windows.Forms.Label label_YingNaShui;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox resultBox_GeRenSuoDe;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.Button button_ShuiCha;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_YingNaShui;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button_msImp;
        private System.Windows.Forms.Label label_statu;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioButton_forg;
        private System.Windows.Forms.RadioButton radioButton_zg;
        private System.Windows.Forms.Button button_imGs;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
    }
}

