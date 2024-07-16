using System;
using System.Drawing;
using System.Windows.Forms;

namespace 珠心算出題_加減_
{
    public partial class Form1 : Form
    {
        private readonly Random _rnd = new Random();
        private readonly int[,] _memory = new int[40, 8]; // 最多 40 題，每排最多 8 字
        private readonly Pen _mPen = new Pen(Color.Black, 1);
        private readonly Pen _noPen = new Pen(Color.Black, 2);
        private readonly Font _drawFont = new Font("Abacus_Fonts", 16F); // 字體
        private readonly Font _noFont = new Font("Arial", 10F); // 字體
        private readonly SolidBrush _drawBrush = new SolidBrush(Color.Black);
        private readonly SolidBrush _whiteBrush = new SolidBrush(Color.White);
        private readonly StringFormat _strFormat = new StringFormat { Alignment = StringAlignment.Far };

        private Bitmap _flag;
        private ComboBox _difficultyComboBox;
        private Button toggleAnswerButton;
        private bool showAnswers = false;
        public Form1()
        {
            InitializeComponent();
            InitializeComboBox(); 
            InitializeToggleAnswerButton();
        }
        private void InitializeToggleAnswerButton()
        {
            toggleAnswerButton = new Button
            {
                Text = "顯示答案",
                Location = new Point(200, 10),
                Size = new Size(100, 30)
            };
            toggleAnswerButton.Click += ToggleAnswerButton_Click;
            Controls.Add(toggleAnswerButton);
        }
        private void ToggleAnswerButton_Click(object sender, EventArgs e)
        {
            showAnswers = !showAnswers;
            toggleAnswerButton.Text = showAnswers ? "隱藏答案" : "顯示答案";
            ShowOrHideAnswers(); // 顯示或隱藏答案
            pictureBox1.Image = _flag; // 重新繪製圖片
        }
        private void ShowOrHideAnswers()
        {
            using (Graphics flagGraphics = Graphics.FromImage(_flag))
            {
                int startX = 60;
                int startY = 70; // 起始Y坐標
                int cellWidth = 50;
                int cellHeight = 40;
                int numCols = 10;
                int numRows = 3;

                for (int l = 0; l < 4; l++)
                {
                    int yOffset = startY + l * (numRows + 2) * cellHeight;
                    for (int j = 0; j < numCols; j++)
                    {
                        if (showAnswers)
                        {
                            int ans = 0;
                            for (int i = 0; i < numRows; i++)
                            {
                                ans += _memory[j + l * 10, i];
                            }
                            flagGraphics.DrawString(ans.ToString(), _drawFont, _drawBrush, startX + j * cellWidth + cellWidth / 2, yOffset + numRows * cellHeight + 10, new StringFormat { Alignment = StringAlignment.Center });
                        }
                        else
                        {
                            flagGraphics.FillRectangle(_whiteBrush, new Rectangle(startX + j * cellWidth + 1, yOffset + numRows * cellHeight + 10, cellWidth - 2, cellHeight - 10));
                        }
                    }
                }
            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void InitializeComboBox()
        {
            _difficultyComboBox = new ComboBox
            {
                Location = new Point(10, 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            for (int i = 1; i <= 11; i++)
            {
                _difficultyComboBox.Items.Add($"難度級數 {i}");
            }

            _difficultyComboBox.SelectedIndex = 0;
            Controls.Add(_difficultyComboBox);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InitializeBitmap();
            ClearMemory();
            if(_difficultyComboBox.SelectedIndex!=10)
            {
                DrawGrid();
            }
            GenerateQuestions(_difficultyComboBox.SelectedIndex + 1);
            pictureBox1.Image = _flag;
        }

        private void InitializeBitmap()
        {
            _flag = new Bitmap(601, 850);
            pictureBox1.Size = new Size(601, 850);
            Controls.Add(pictureBox1);
        }

        private void ClearMemory()
        {
            for (int i = 0; i < 40; i++)
                for (int j = 0; j < 8; j++)
                    _memory[i, j] = 0;
        }

        private void DrawGrid()
        {
            using (Graphics flagGraphics = Graphics.FromImage(_flag))
            {
                flagGraphics.Clear(Color.White);

                flagGraphics.DrawLine(_noPen, 1, 1, 600, 1); // 畫外框
                flagGraphics.DrawLine(_noPen, 1, 1, 1, 840);
                flagGraphics.DrawLine(_noPen, 1, 840, 600, 840);
                flagGraphics.DrawLine(_noPen, 600, 1, 600, 840);

                flagGraphics.DrawLine(_mPen, 1, 20, 600, 20);

                flagGraphics.DrawLine(_noPen, 1, 210, 600, 210); // 中央上橫線
                flagGraphics.DrawLine(_mPen, 1, 170 + 20 - 10, 600, 170 + 20 - 10); // 第一行解答上橫線
                flagGraphics.DrawLine(_mPen, 1, 210 + 20, 600, 210 + 20); // 第二行題目上橫線

                flagGraphics.DrawLine(_noPen, 1, 420, 600, 420); // 正中央橫線
                flagGraphics.DrawLine(_mPen, 1, 420 + 20, 600, 420 + 20); // 第三行題目下橫線
                flagGraphics.DrawLine(_mPen, 1, 370 + 20, 600, 370 + 20); // 第二行解答上橫線

                flagGraphics.DrawLine(_noPen, 1, 630, 600, 630); // 中央下橫線
                flagGraphics.DrawLine(_mPen, 1, 580 + 20, 600, 580 + 20); // 第三行解答上橫線
                flagGraphics.DrawLine(_mPen, 1, 630 + 20, 600, 630 + 20); // 第四行題目下橫線

                flagGraphics.DrawLine(_mPen, 1, 810, 600, 810); // 第四行解答上橫線
                flagGraphics.DrawLine(_mPen, 1, 840, 600, 840); // 第四行解答下橫線

                for (int i = 1; i <= 4; i++)
                {
                    int yOffset = 210 * (i - 1);
                    DrawHeaders(flagGraphics, yOffset);
                }

                // 繪製垂直線
                for (int i = 0; i < 10; i++)
                {
                    flagGraphics.DrawLine(_mPen, 30 + 57 * i, 1, 30 + 57 * i, 840);
                }
            }
        }

        private void DrawHeaders(Graphics flagGraphics, int yOffset)
        {
            for (int i = 0; i < 8; i++)
                flagGraphics.DrawString((i + 1).ToString(), _noFont, _drawBrush, 11, yOffset + 20 + i * 20);

            flagGraphics.DrawString("Ans", _noFont, _drawBrush, 4, yOffset + 187);
            flagGraphics.DrawString("NO", _noFont, _drawBrush, 5, 2 + yOffset);

            for (int i = 0; i < 10; i++)
            {
                int x = i < 9 ? 54 + 57 * i : 52 + 57 * i;
                flagGraphics.DrawString((i + 1).ToString(), _noFont, _drawBrush, x, yOffset + 3);
            }
        }

        private void GenerateQuestions(int difficultyLevel)
        {
            switch (difficultyLevel)
            {
                case 1:
                    GenerateLevel1Questions();
                    break;
                case 2:
                    GenerateLevel2Questions();
                    break;
                case 3:
                    GenerateLevel3Questions();
                    break;
                case 4:
                    GenerateLevel4Questions();
                    break;
                case 5:
                    GenerateLevel5Questions();
                    break;
                case 6:
                    GenerateLevel6Questions();
                    break;
                case 7:
                    GenerateLevel7Questions();
                    break;
                case 8:
                    GenerateLevel8Questions();
                    break;
                case 9:
                    GenerateLevel9Questions();
                    break;
                case 10:
                    GenerateLevel10Questions();
                    break;
                case 11:
                    GenerateLevel11Questions();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void GenerateLevel1Questions()
        {
            // Add your code for Level 1 questions
        }

        private void GenerateLevel2Questions()
        {
            // Add your code for Level 2 questions
        }

        private void GenerateLevel3Questions()
        {
            // Add your code for Level 3 questions
        }

        private void GenerateLevel4Questions()
        {

            using (Graphics flagGraphics = Graphics.FromImage(_flag))
            {
                flagGraphics.Clear(Color.White);
                int times = 8;
                double max = 99;
                int NO_height = 20;
                int lastMinus = 0;
                int minus = 0;

                for (int l = 0; l < 4; l++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        int ans = 0;
                        int minus_check_ans = 0;
                        int minus_quest = 0;

                        if (j == 0 || j == 2 || j == 4 || j == 5 || j == 7 || j == 9)
                        {
                            for (int i = 0; i < times; i++)
                            {
                                int random = _rnd.Next(10, (int)max);
                                ans += random;
                                _memory[j, i] = random;
                                flagGraphics.DrawString(random.ToString(), _drawFont, _drawBrush, 87 + 57 * j, 210 * l + NO_height + i * 20, _strFormat);
                                flagGraphics.DrawLine(_mPen, 30 + 57 * j, 1, 30 + 57 * j, 840);
                            }
                        }
                        else if (j == 1 || j == 3 || j == 6 || j == 8)
                        {
                            for (int i = 0; i < times; i++)
                            {
                                int random = _rnd.Next(10, (int)max);
                                ans += random;
                                _memory[j, i] = random;
                                flagGraphics.DrawString(random.ToString(), _drawFont, _drawBrush, 87 + 57 * j, 210 * l + NO_height + i * 20, _strFormat);
                                flagGraphics.DrawLine(_mPen, 30 + 57 * j, 1, 30 + 57 * j, 840);
                            }

                        OneMoreTime:
                            minus = _rnd.Next(1, times);

                            if (minus == lastMinus)
                                goto OneMoreTime;
                            minus_check_ans = 0;
                            for (int i = 0; i < minus - 1; i++)
                            {
                                minus_check_ans += _memory[j, i];
                            }
                            if (minus_quest >= 2)
                            {
                                continue;
                            }
                            else if (minus_check_ans >= _memory[j, minus] && minus_quest < 2)
                            {
                                _memory[j, minus] = _memory[j, minus] * -1;
                                flagGraphics.FillRectangle(_whiteBrush, new Rectangle(67 + 57 * j, NO_height + minus * 20 + 210 * l, 20, 19));
                                flagGraphics.DrawString(_memory[j, minus].ToString(), _drawFont, _drawBrush, 87 + 57 * j, 210 * l + NO_height + minus * 20, _strFormat);
                                minus_quest = minus_quest + 1;
                                lastMinus = minus;
                                goto OneMoreTime;
                            }
                            else if (minus_quest < 2)
                            {
                                goto OneMoreTime;
                            }
                        }
                    }
                }
            }
        }

        private void GenerateLevel5Questions()
        {
            // Add your code for Level 5 questions
        }

        private void GenerateLevel6Questions()
        {

        }

        private void GenerateLevel7Questions()
        {
            // Add your code for Level 7 questions
        }

        private void GenerateLevel8Questions()
        {
            // Add your code for Level 8 questions
        }

        private void GenerateLevel9Questions()
        {
            // Add your code for Level 9 questions
        }

        private void GenerateLevel10Questions()
        {
            // Add your code for Level 10 questions
        }

        private void GenerateLevel11Questions()
        {
            using (Graphics flagGraphics = Graphics.FromImage(_flag))
            {
                flagGraphics.Clear(Color.White); // 設置背景為白色

                int startX = 60;
                int startY = 70; // 調整起始Y坐標以留出更多空間顯示題號和框線
                int cellWidth = 50;
                int cellHeight = 40;
                int numCols = 10;
                int numRows = 3;
                int totalRows = 4 * (numRows + 2);

                for (int l = 0; l < 4; l++)
                {
                    int yOffset = startY + l * (numRows + 2) * cellHeight;
                    for (int j = 0; j < numCols; j++)
                    {
                        // 繪製題號
                        flagGraphics.DrawString((l * numCols + j + 1).ToString(), _noFont, _drawBrush, startX + j * cellWidth + cellWidth / 2, yOffset - cellHeight, new StringFormat { Alignment = StringAlignment.Center });

                        int ans = 0;
                        for (int i = 0; i < numRows; i++)
                        {
                            int random = _rnd.Next(1, 10);
                            _memory[j + l * 10, i] = random;  // 正確記錄數據
                            ans += random;
                            flagGraphics.DrawString(random.ToString(), _drawFont, _drawBrush, startX + j * cellWidth + cellWidth / 2, yOffset + i * cellHeight + 10, new StringFormat { Alignment = StringAlignment.Center });
                        }
                    }

                    // 繪製垂直線
                    for (int i = 0; i <= numCols; i++)
                    {
                        flagGraphics.DrawLine(_mPen, startX + i * cellWidth, yOffset - cellHeight, startX + i * cellWidth, yOffset + (numRows + 1) * cellHeight);
                    }

                    // 繪製區隔上下題目的橫線
                    flagGraphics.DrawLine(_mPen, startX, yOffset - cellHeight+15, startX + cellWidth * numCols, yOffset - cellHeight+15); // 題號區的橫線 *
                    flagGraphics.DrawLine(_mPen, startX, yOffset + numRows * cellHeight, startX + cellWidth * numCols, yOffset + numRows * cellHeight); // 答案區上方的橫線
                    flagGraphics.DrawLine(_mPen, startX, yOffset + (numRows + 1) * cellHeight, startX + cellWidth * numCols, yOffset + (numRows + 1) * cellHeight); // 區隔上下題目的橫線

                    // 添加額外的橫線
                    //flagGraphics.DrawLine(_mPen, startX, yOffset - cellHeight - 20, startX + cellWidth * numCols, yOffset - cellHeight - 20);

                    flagGraphics.DrawString("NO", _noFont, _drawBrush, startX - 30, yOffset + 10);
                    flagGraphics.DrawString("Ans", _noFont, _drawBrush, startX - 30, yOffset + numRows * cellHeight + 10);
                }
                // 繪製外圍黑色粗框
                flagGraphics.DrawRectangle(_noPen, startX - 1, startY - cellHeight - 21+21, cellWidth * numCols + 2, cellHeight * totalRows + 2);

            }
        }

        private int CalculateMinusCheckAns(int j, int minus)
        {
            int minusCheckAns = 0;
            for (int i = 0; i < minus - 1; i++)
                minusCheckAns += _memory[j, i];
            return minusCheckAns;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog
            {
                FileName = "題目",
                Filter = "(.png)|*.png"
            };

            if (save.ShowDialog() == DialogResult.OK)
            {
                using (Bitmap bit = new Bitmap(pictureBox1.ClientRectangle.Width, pictureBox1.ClientRectangle.Height))
                {
                    pictureBox1.DrawToBitmap(bit, pictureBox1.ClientRectangle);
                    bit.Save(save.FileName);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _flag?.Save("D:/Program Item/test.png");
        }
    }
}
