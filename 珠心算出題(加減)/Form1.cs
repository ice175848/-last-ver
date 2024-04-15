﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace 珠心算出題_加減_
{
    public partial class Form1 : Form
    {
        Random rnd = new Random();
        int[,] memory = new int[40, 8];//最多40題 每排最多8字 
        Pen mPen = new Pen(Color.Black, 1);
        Pen NoPen = new Pen(Color.Black, 2);
        Font drawFont = new Font("新細明體", 16F);//字體
        Font No_Font = new Font("Arial", 10F);//字體
        SolidBrush drawBrush = new SolidBrush(Color.Black);
        SolidBrush whiteBrush = new SolidBrush(Color.White);

        StringFormat strFormat = new StringFormat();
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        Bitmap flag;
        private void button1_Click(object sender, EventArgs e)
        {
            flag = new Bitmap(601, 850);
            pictureBox1.Size = new Size(601, 850);
            this.Controls.Add(pictureBox1);

            Graphics flagGraphics = Graphics.FromImage(flag);

            for (int i = 0; i < 40; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    memory[i, j] = 0;
                }
            }
            int times = 0;
            double max, min;
            int questionNUM = 0;

            int NO_height = 20;
            
            flagGraphics.Clear(Color.White);

            strFormat.Alignment = StringAlignment.Far;

            flagGraphics.DrawLine(NoPen, 1, 1, 600, 1);
            flagGraphics.DrawLine(NoPen, 1, 1, 1, 840);
            flagGraphics.DrawLine(NoPen, 1, 840, 600, 840);
            flagGraphics.DrawLine(NoPen, 600, 1, 600, 840);

            flagGraphics.DrawLine(mPen, 1, NO_height, 600, NO_height);

            flagGraphics.DrawLine(NoPen, 1, 210, 600, 210);//中央上橫線
            flagGraphics.DrawLine(mPen, 1, 170 + NO_height - 10, 600, 170 + NO_height - 10);//第一行解答上橫線
            flagGraphics.DrawLine(mPen, 1, 210 + NO_height, 600, 210 + NO_height);//第二行題目上橫線

            flagGraphics.DrawLine(NoPen, 1, 420, 600, 420);//正中央橫線
            flagGraphics.DrawLine(mPen, 1, 420 + NO_height, 600, 420 + NO_height);//第三行題目下橫線
            flagGraphics.DrawLine(mPen, 1, 370 + NO_height, 600, 370 + NO_height);//第二行解答上橫線
            
            flagGraphics.DrawLine(NoPen, 1, 630, 600, 630);//中央下橫線
            flagGraphics.DrawLine(mPen, 1, 580 + NO_height, 600, 580 + NO_height);//第三行解答上橫線
            flagGraphics.DrawLine(mPen, 1, 630 + NO_height, 600, 630 + NO_height);//第四行題目下橫線

            flagGraphics.DrawLine(mPen, 1, 810, 600, 810);//第四行解答上橫線
            flagGraphics.DrawLine(mPen, 1, 840, 600, 840);//第四行解答上橫線

            for (int l = 0; l < 4; l++)
            {
                for (int i = 0; i < 8; i++)
                {
                    flagGraphics.DrawString((i + 1).ToString(), No_Font, drawBrush, 11, 210 * l + NO_height + i * 20);//寫上口數
                }

                flagGraphics.DrawString("Ans", No_Font, drawBrush, 4, 210 * l + 187);
                flagGraphics.DrawString("NO", No_Font, drawBrush, 5, 2 + 210 * l);

                for (int i = 0; i < 10; i++)
                {
                    if (l * 10 + i + 1 < 10)//比10小的話
                    {
                        flagGraphics.DrawString((l * 10 + i + 1).ToString(), No_Font, drawBrush, 54 + 57 * i, 210 * l + 3);//寫上1~9
                    }
                    else//比10大的話
                    {
                        flagGraphics.DrawString((l * 10 + i + 1).ToString(), No_Font, drawBrush, 52 + 57 * i, 210 * l + 3);//寫上10~40
                    }
                }
            }

            max = 99;
            min = 10;

            times = 8;

            int lastMinus = 0;
            int minus = 0;
            for (int l = 0; l < 4; l++)
            {
                for (int j = 0; j < 10; j++)//2021/05/06
                {
                    int ans = 0;
                    int minus_check_ans = 0;
                    int minus_quest = 0;
                    if (j == 0 || j == 2 || j == 4 || j == 5 || j == 7 || j == 9)//第1.3.5.6.8.10題(全加)
                    {
                        for (int i = 0; i < times; i++)
                        {
                            int random = rnd.Next(10, (Int32)max);
                            ans += random;
                            memory[j, i] = random;
                            flagGraphics.DrawString(random.ToString(), drawFont, drawBrush, 87 + 57 * j, 210 * l + NO_height + i * 20, strFormat);
                            flagGraphics.DrawLine(mPen, 30 + 57 * j, 1, 30 + 57 * j, 840);
                        }
                    }
                    else if (j == 1 || j == 3 || j == 6 || j == 8)//第2.4.7.9題(有減)
                    {
                        for (int i = 0; i < times; i++)
                        {
                            int random = rnd.Next(10, (Int32)max);
                            ans += random;
                            memory[j, i] = random;
                            flagGraphics.DrawString(random.ToString(), drawFont, drawBrush, 87 + 57 * j, 210 * l + NO_height + i * 20, strFormat);

                            flagGraphics.DrawLine(mPen, 30 + 57 * j, 1, 30 + 57 * j, 840);
                        }
                    OneMoreTime:
                        minus = rnd.Next(1, times);//生成隨機題號

                        if (minus == lastMinus)
                            goto OneMoreTime;
                        minus_check_ans = 0;
                        for (int i = 0; i < minus - 1; i++)
                        {
                            minus_check_ans += memory[j, i];//累加答案
                        }
                        if (minus_quest >= 2)
                        {
                            continue;
                        }
                        else if (minus_check_ans >= memory[j, minus] && minus_quest < 2)//如果負數還沒滿兩題，且夠減的話
                        {
                            memory[j, minus] = memory[j, minus] * -1;//該數變負數
                            flagGraphics.FillRectangle(whiteBrush, new Rectangle(67 + 57 * j, NO_height + minus * 20 + 210 * l, 20, 19));
                            flagGraphics.DrawString(memory[j, minus].ToString(), drawFont, drawBrush, 87 + 57 * j, 210 * l + NO_height + minus * 20, strFormat);
                            minus_quest = minus_quest + 1;//累積數+1
                            lastMinus = minus;
                            goto OneMoreTime;
                        }
                        else if (minus_quest < 2)
                        {
                            goto OneMoreTime;
                        }
                    }
                }
                if (questionNUM >= 10)
                {
                    questionNUM = 0;

                    break;
                }
                questionNUM = questionNUM + 1;
            }

            pictureBox1.Image = flag;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.FileName = "題目";
            save.Filter = "(.png)|*.png";

            if (save.ShowDialog() == DialogResult.OK)
            {
                Bitmap bit = new Bitmap(pictureBox1.ClientRectangle.Width, pictureBox1.ClientRectangle.Height);
                pictureBox1.DrawToBitmap(bit, pictureBox1.ClientRectangle);
                bit.Save(save.FileName);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            flag.Save("D:/Program Item/test.png");
        }
    }
}
