using System;
using System.Collections.Generic;
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
        private Font _drawFont; // 字體
        private readonly Font _noFont = new Font("Arial", 10F); // 字體
        private int question_gap;
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
            toggleAnswerButton.Enabled = false;
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
        private bool CheckForNegative(int j, int numberOfDigits)
        {
            int sum = 0;
            for (int i = 0; i < numberOfDigits; i++)
            {
                sum += _memory[j, i];
                if (sum < 0)
                {
                    return true; // 計算過程中出現負數
                }
            }
            return false; // 計算過程中沒有出現負數
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
            _difficultyComboBox.SelectedIndexChanged += DifficultyComboBox_SelectedIndexChanged;

            Controls.Add(_difficultyComboBox);
        }
        private void DifficultyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedDifficulty = _difficultyComboBox.SelectedIndex + 1;

            switch (selectedDifficulty)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    numericUpDown1.Value = 12;
                    numericUpDown2.Enabled = false;
                    break;
                case 5:
                    numericUpDown1.Value = 13;
                    numericUpDown2.Value = 20;
                    break;
                case 6:
                    numericUpDown1.Value = 13;
                    numericUpDown2.Value = 19; 
                    break;
                case 7:
                    numericUpDown1.Value = 13;
                    numericUpDown2.Value = 22;
                    break;
                case 8:
                    numericUpDown1.Value = 15;
                    numericUpDown2.Value = 25;
                    break;
                case 9:
                    numericUpDown1.Value = 16;
                    numericUpDown2.Value = 32;
                    break;
                case 10:
                case 11:
                default:
                    question_gap = 30; // 默認值
                    break;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            toggleAnswerButton.Enabled = true;
            _drawFont = new Font(comboBox1.Text, float.Parse(numericUpDown1.Text)); // 字體
            question_gap = (int)numericUpDown2.Value;

            InitializeBitmap();
            ClearMemory();
            if(_difficultyComboBox.SelectedIndex!=10)//11級不要畫背景 因為背景不同
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
                DrawGrid();
                for (int l = 0; l < 4; l++)//4排題目
                {
                    for (int j = 0; j < 10; j++)//每排10題
                    {
                        int ans = 0;
                        int minus_check_ans = 0;
                        int minus_quest = 0;

                        if (j == 0 || j == 2 || j == 4 || j == 5 || j == 7 || j == 9)//第1,3,5,6,8,10題要是正的
                        {
                            for (int i = 0; i < times; i++)
                            {
                                int random = _rnd.Next(10, (int)max);
                                ans += random;
                                _memory[j, i] = random;
                                flagGraphics.DrawString(random.ToString(), _drawFont, _drawBrush, 87 + 57 * j, 210 * l + NO_height + i * 20+2, _strFormat);
                                flagGraphics.DrawLine(_mPen, 30 + 57 * j, 1, 30 + 57 * j, 840);
                            }
                        }
                        else if (j == 1 || j == 3 || j == 6 || j == 8)//第2,4,7,9題是有負數的
                        {
                            for (int i = 0; i < times; i++)
                            {
                                int random = _rnd.Next(10, (int)max);
                                ans += random;
                                _memory[j, i] = random;
                                flagGraphics.DrawString(random.ToString(), _drawFont, _drawBrush, 87 + 57 * j, 210 * l + NO_height + i * 20+2, _strFormat);
                                flagGraphics.DrawLine(_mPen, 30 + 57 * j, 1, 30 + 57 * j, 840);
                            }
                        //如果是計算過程產生負 重新考慮排負位置
                        OneMoreTime:
                            minus = _rnd.Next(1, times);//負數的位置在其中一口數

                            if (minus == lastMinus)
                                goto OneMoreTime;
                            minus_check_ans = 0;
                            for (int i = 0; i < minus - 1; i++)
                            {
                                minus_check_ans += _memory[j, i];
                            }
                            if (minus_quest >= 2)//如果生成負數
                            {
                                continue;
                            }
                            else if (minus_check_ans >= _memory[j, minus] && minus_quest < 2)
                            {
                                _memory[j, minus] = _memory[j, minus] * -1;
                                flagGraphics.FillRectangle(_whiteBrush, new Rectangle(67 + 57 * j, NO_height + minus * 20 + 210 * l, 20, 19));
                                flagGraphics.DrawString(_memory[j, minus].ToString(), _drawFont, _drawBrush, 87 + 57 * j, 210 * l + NO_height + minus * 20+2, _strFormat);
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
            toggleAnswerButton.Enabled = false;
            toggleAnswerButton.Text = "暫未開放";
            using (Graphics flagGraphics = Graphics.FromImage(_flag))
            {
                flagGraphics.Clear(Color.White);
                int times1 = 8; // 1-5題的口數
                int times2 = 6; // 6-10題的口數
                double max1 = 10; // 一位數的最大值
                double max2 = 100; // 二位數的最大值
                int NO_height = 30;
                //int question_gap = 25;

                DrawGrid();

                for (int l = 0; l < 4; l++)//4排題目
                {
                    for (int j = 0; j < 10; j++)//每排10題
                    {
                        int ans = 0;
                        int minus_check_ans = 0;
                        int minus_quest = 0;

                        if (j == 0 || j == 2 || j == 4)//第1,3,5題數字順序固定是2位數,1位數,2位數...以此類推
                        {
                            do
                            {
                                for (int i = 0; i < times1; i++)
                                {
                                    if (i % 2 == 0)
                                    {
                                        _memory[j, i] = _rnd.Next(10, (int)max2);
                                    }
                                    else
                                    {
                                        _memory[j, i] = _rnd.Next(1, (int)max1);
                                    }
                                }
                            } while (CheckForNegative(j, times1));

                            for (int i = 0; i < times1; i++)
                            {
                                flagGraphics.DrawString(_memory[j, i].ToString(), _drawFont, _drawBrush, 87 + 57 * j, 210 * l + NO_height + i * question_gap-8, _strFormat);
                            }
                            flagGraphics.DrawLine(_mPen, 30 + 57 * j, 1, 30 + 57 * j, 840);
                        }
                        else if (j == 1 || j == 3)//第2,4題數字順序固定是1位數,2位數,1位數...以此類推
                        {
                            do
                            {
                                for (int i = 0; i < times1; i++)
                                {
                                    if (i % 2 == 0)
                                    {
                                        _memory[j, i] = _rnd.Next(1, (int)max1);
                                    }
                                    else
                                    {
                                        _memory[j, i] = _rnd.Next(10, (int)max2);
                                    }
                                }
                                int firstMinus = _rnd.Next(0, times1);
                                int secondMinus;
                                do
                                {
                                    secondMinus = _rnd.Next(0, times1);
                                } while (secondMinus == firstMinus);

                                _memory[j, firstMinus] *= -1;
                                _memory[j, secondMinus] *= -1;
                            } while (CheckForNegative(j, times1));

                            for (int i = 0; i < times1; i++)
                            {
                                flagGraphics.DrawString(_memory[j, i].ToString(), _drawFont, _drawBrush, 87 + 57 * j, 210 * l + NO_height + i * question_gap-8, _strFormat);
                            }
                            flagGraphics.DrawLine(_mPen, 30 + 57 * j, 1, 30 + 57 * j, 840);
                        }
                        else if (j == 5 || j == 7 || j == 9)//第6,8,10題是二位數六口
                        {
                            do
                            {
                                for (int i = 0; i < times2; i++)
                                {
                                    _memory[j, i] = _rnd.Next(10, (int)max2);
                                }
                            } while (CheckForNegative(j, times2));

                            for (int i = 0; i < times2; i++)
                            {
                                flagGraphics.DrawString(_memory[j, i].ToString(), _drawFont, _drawBrush, 87 + 57 * j, 210 * l + NO_height + (i+1) * question_gap-8, _strFormat);
                            }
                            flagGraphics.DrawLine(_mPen, 30 + 57 * j, 1, 30 + 57 * j, 840);
                        }
                        else if (j == 6 || j == 8)//第7,9題是有負數的二位數六口
                        {
                            do
                            {
                                for (int i = 0; i < times2; i++)
                                {
                                    _memory[j, i] = _rnd.Next(10, (int)max2);
                                }
                                int firstMinus = _rnd.Next(0, times2);
                                int secondMinus;
                                do
                                {
                                    secondMinus = _rnd.Next(0, times2);
                                } while (secondMinus == firstMinus);

                                _memory[j, firstMinus] *= -1;
                                _memory[j, secondMinus] *= -1;
                            } while (CheckForNegative(j, times2));

                            for (int i = 0; i < times2; i++)
                            {
                                flagGraphics.DrawString(_memory[j, i].ToString(), _drawFont, _drawBrush, 87 + 57 * j, 210 * l + NO_height + (i+1) * question_gap-8, _strFormat);
                            }
                            flagGraphics.DrawLine(_mPen, 30 + 57 * j, 1, 30 + 57 * j, 840);
                        }
                    }
                }
            }
        }

        private void GenerateLevel6Questions()
        {
            toggleAnswerButton.Enabled = false;
            toggleAnswerButton.Text = "暫未開放";
            using (Graphics flagGraphics = Graphics.FromImage(_flag))
            {
                flagGraphics.Clear(Color.White);
                int times1 = 8; // 1-5題的口數
                int times2 = 5; // 6-10題的口數
                double max1 = 10; // 一位數的最大值
                double max2 = 100; // 二位數的最大值
                int NO_height = 30;


                DrawGrid();

                for (int l = 0; l < 4; l++)//4排題目
                {
                    for (int j = 0; j < 10; j++)//每排10題
                    {
                        int ans = 0;
                        int minus_check_ans = 0;
                        int minus_quest = 0;

                        if (j == 0 || j == 2 || j == 4)//第1,3,5題要是正的一位數八口
                        {
                            do
                            {
                                for (int i = 0; i < times1; i++)
                                {
                                    int random = _rnd.Next(1, (int)max1);
                                    _memory[j, i] = random;
                                }
                            } while (CheckForNegative(j, times1));

                            for (int i = 0; i < times1; i++)
                            {
                                flagGraphics.DrawString(_memory[j, i].ToString(), _drawFont, _drawBrush, 87 + 57 * j, 210 * l + NO_height + i * question_gap, _strFormat);
                            }
                            flagGraphics.DrawLine(_mPen, 30 + 57 * j, 1, 30 + 57 * j, 840);
                        }
                        else if (j == 5 || j == 7 || j == 9)//第6,8,10題是正的一位兩口+二位三口
                        {
                            do
                            {
                                List<int> positions = new List<int> { 0, 1, 2, 3, 4 };
                                for (int i = 0; i < times2; i++)
                                {
                                    if (positions.Count > 0)
                                    {
                                        int randomIndex = _rnd.Next(positions.Count);
                                        int position = positions[randomIndex];
                                        positions.RemoveAt(randomIndex);

                                        if (i < 2)
                                        {
                                            _memory[j, position] = _rnd.Next(1, (int)max1);
                                        }
                                        else
                                        {
                                            _memory[j, position] = _rnd.Next(10, (int)max2);
                                        }
                                    }
                                }
                            } while (CheckForNegative(j, times2));

                            for (int i = 0; i < times2; i++)
                            {
                                flagGraphics.DrawString(_memory[j, i].ToString(), _drawFont, _drawBrush, 87 + 57 * j, 210 * l + NO_height + (i + 1) * question_gap, _strFormat);
                            }
                            flagGraphics.DrawLine(_mPen, 30 + 57 * j, 1, 30 + 57 * j, 840);
                        }
                        else if (j == 1 || j == 3)//第2,4題是有負數的一位數八口
                        {
                            do
                            {
                                for (int i = 0; i < times1; i++)
                                {
                                    int random = _rnd.Next(1, (int)max1);
                                    _memory[j, i] = random;
                                }
                                int firstMinus = _rnd.Next(1, times1);
                                int secondMinus;
                                do
                                {
                                    secondMinus = _rnd.Next(1, times1);
                                } while (secondMinus == firstMinus);

                                _memory[j, firstMinus] *= -1;
                                _memory[j, secondMinus] *= -1;
                            } while (CheckForNegative(j, times1));

                            for (int i = 0; i < times1; i++)
                            {
                                flagGraphics.DrawString(_memory[j, i].ToString(), _drawFont, _drawBrush, 87 + 57 * j, 210 * l + NO_height + i * question_gap, _strFormat);
                            }
                            flagGraphics.DrawLine(_mPen, 30 + 57 * j, 1, 30 + 57 * j, 840);
                        }
                        else if (j == 6 || j == 8)//第7,9題是正的二位三口+正的一位兩口+負的一位兩口
                        {
                            do
                            {
                                List<int> positions = new List<int> { 0, 1, 2, 3, 4 };
                                for (int i = 0; i < times2; i++)
                                {
                                    if (positions.Count > 0)
                                    {
                                        int randomIndex = _rnd.Next(positions.Count);
                                        int position = positions[randomIndex];
                                        positions.RemoveAt(randomIndex);

                                        if (i < 2)
                                        {
                                            _memory[j, position] = _rnd.Next(1, (int)max1);
                                        }
                                        else
                                        {
                                            _memory[j, position] = _rnd.Next(10, (int)max2);
                                        }
                                    }
                                }

                                int firstMinus = _rnd.Next(0, times2);
                                int secondMinus;
                                do
                                {
                                    secondMinus = _rnd.Next(0, times2);
                                } while (secondMinus == firstMinus);

                                _memory[j, firstMinus] *= -1;
                                _memory[j, secondMinus] *= -1;
                            } while (CheckForNegative(j, times2));

                            for (int i = 0; i < times2; i++)
                            {
                                flagGraphics.DrawString(_memory[j, i].ToString(), _drawFont, _drawBrush, 87 + 57 * j, 210 * l + NO_height + (i + 1) * question_gap, _strFormat);
                            }
                            flagGraphics.DrawLine(_mPen, 30 + 57 * j, 1, 30 + 57 * j, 840);
                        }
                    }
                }
            }
        }


        private void GenerateLevel7Questions()
        {
            toggleAnswerButton.Enabled = false;
            toggleAnswerButton.Text = "暫未開放";
            using (Graphics flagGraphics = Graphics.FromImage(_flag))
            {
                flagGraphics.Clear(Color.White);
                int times1 = 7; // 1-5題的口數
                int times2 = 5; // 6-10題的口數
                double max1 = 10; // 一位數的最大值
                double max2 = 100; // 二位數的最大值
                int NO_height = 30;
                //int question_gap = 23;

                DrawGrid();

                for (int l = 0; l < 4; l++)//4排題目
                {
                    for (int j = 0; j < 10; j++)//每排10題
                    {
                        int ans = 0;
                        int minus_check_ans = 0;
                        int minus_quest = 0;

                        if (j == 0 || j == 2 || j == 4)//第1,3,5題要是正的一位數七口
                        {
                            do
                            {
                                for (int i = 0; i < times1; i++)
                                {
                                    int random = _rnd.Next(1, (int)max1);
                                    _memory[j, i] = random;
                                }
                            } while (CheckForNegative(j, times1));

                            for (int i = 0; i < times1; i++)
                            {
                                flagGraphics.DrawString(_memory[j, i].ToString(), _drawFont, _drawBrush, 87 + 57 * j, 210 * l + NO_height + i * question_gap, _strFormat);
                            }
                            flagGraphics.DrawLine(_mPen, 30 + 57 * j, 1, 30 + 57 * j, 840);
                        }
                        else if (j == 5 || j == 7 || j == 9)//第6,8,10題是正的一位三口+二位兩口
                        {
                            do
                            {
                                List<int> positions = new List<int> { 0, 1, 2, 3, 4 };
                                for (int i = 0; i < times2; i++)
                                {
                                    if (positions.Count > 0)
                                    {
                                        int randomIndex = _rnd.Next(positions.Count);
                                        int position = positions[randomIndex];
                                        positions.RemoveAt(randomIndex);

                                        if (i < 3)
                                        {
                                            _memory[j, position] = _rnd.Next(1, (int)max1);
                                        }
                                        else
                                        {
                                            _memory[j, position] = _rnd.Next(10, (int)max2);
                                        }
                                    }
                                }
                            } while (CheckForNegative(j, times2));

                            for (int i = 0; i < times2; i++)
                            {
                                flagGraphics.DrawString(_memory[j, i].ToString(), _drawFont, _drawBrush, 87 + 57 * j, 210 * l + NO_height + (i + 1) * question_gap, _strFormat);
                            }
                            flagGraphics.DrawLine(_mPen, 30 + 57 * j, 1, 30 + 57 * j, 840);
                        }
                        else if (j == 1 || j == 3)//第2,4題是有負數的一位數七口
                        {
                            do
                            {
                                for (int i = 0; i < times1; i++)
                                {
                                    int random = _rnd.Next(1, (int)max1);
                                    _memory[j, i] = random;
                                }
                                int firstMinus = _rnd.Next(1, times1);
                                int secondMinus;
                                do
                                {
                                    secondMinus = _rnd.Next(1, times1);
                                } while (secondMinus == firstMinus);

                                _memory[j, firstMinus] *= -1;
                                _memory[j, secondMinus] *= -1;
                            } while (CheckForNegative(j, times1));

                            for (int i = 0; i < times1; i++)
                            {
                                flagGraphics.DrawString(_memory[j, i].ToString(), _drawFont, _drawBrush, 87 + 57 * j, 210 * l + NO_height + i * question_gap, _strFormat);
                            }
                            flagGraphics.DrawLine(_mPen, 30 + 57 * j, 1, 30 + 57 * j, 840);
                        }
                        else if (j == 6 || j == 8)//第7,9題是正的二位兩口+正的一位三口+負的一位兩口
                        {
                            do
                            {
                                List<int> positions = new List<int> { 0, 1, 2, 3, 4 };
                                for (int i = 0; i < times2; i++)
                                {
                                    if (positions.Count > 0)
                                    {
                                        int randomIndex = _rnd.Next(positions.Count);
                                        int position = positions[randomIndex];
                                        positions.RemoveAt(randomIndex);

                                        if (i < 3)
                                        {
                                            _memory[j, position] = _rnd.Next(1, (int)max1);
                                        }
                                        else
                                        {
                                            _memory[j, position] = _rnd.Next(10, (int)max2);
                                        }
                                    }
                                }

                                int firstMinus = _rnd.Next(0, times2);
                                int secondMinus;
                                do
                                {
                                    secondMinus = _rnd.Next(0, times2);
                                } while (secondMinus == firstMinus);

                                _memory[j, firstMinus] *= -1;
                                _memory[j, secondMinus] *= -1;
                            } while (CheckForNegative(j, times2));

                            for (int i = 0; i < times2; i++)
                            {
                                flagGraphics.DrawString(_memory[j, i].ToString(), _drawFont, _drawBrush, 87 + 57 * j, 210 * l + NO_height + (i + 1) * question_gap, _strFormat);
                            }
                            flagGraphics.DrawLine(_mPen, 30 + 57 * j, 1, 30 + 57 * j, 840);
                        }
                    }
                }
            }
        }

        private void GenerateLevel8Questions()
        {
            toggleAnswerButton.Enabled = false;
            toggleAnswerButton.Text = "暫未開放";
            using (Graphics flagGraphics = Graphics.FromImage(_flag))
            {
                flagGraphics.Clear(Color.White);
                int times1 = 6; // 1-5題的口數
                int times2 = 4; // 6-10題的口數
                double max1 = 10; // 一位數的最大值
                double max2 = 100; // 二位數的最大值
                int NO_height = 30;
                //int question_gap = 25;

                DrawGrid();

                for (int l = 0; l < 4; l++)//4排題目
                {
                    for (int j = 0; j < 10; j++)//每排10題
                    {
                        int ans = 0;
                        int minus_check_ans = 0;
                        int minus_quest = 0;

                        if (j == 0 || j == 2 || j == 4)//第1,3,5題要是正的一位數六口
                        {
                            do
                            {
                                for (int i = 0; i < times1; i++)
                                {
                                    int random = _rnd.Next(1, (int)max1);
                                    _memory[j, i] = random;
                                }
                            } while (CheckForNegative(j, times1));

                            for (int i = 0; i < times1; i++)
                            {
                                flagGraphics.DrawString(_memory[j, i].ToString(), _drawFont, _drawBrush, 87 + 57 * j, 210 * l + NO_height + i * question_gap, _strFormat);
                            }
                            flagGraphics.DrawLine(_mPen, 30 + 57 * j, 1, 30 + 57 * j, 840);
                        }
                        else if (j == 5 || j == 7 || j == 9)//第6,8,10題是正的一位二口+二位兩口
                        {
                            do
                            {
                                List<int> positions = new List<int> { 0, 1, 2, 3 };
                                for (int i = 0; i < times2; i++)
                                {
                                    if (positions.Count > 0)
                                    {
                                        int randomIndex = _rnd.Next(positions.Count);
                                        int position = positions[randomIndex];
                                        positions.RemoveAt(randomIndex);

                                        if (i < 2)
                                        {
                                            _memory[j, position] = _rnd.Next(1, (int)max1);
                                        }
                                        else
                                        {
                                            _memory[j, position] = _rnd.Next(10, (int)max2);
                                        }
                                    }
                                }
                            } while (CheckForNegative(j, times2));

                            for (int i = 0; i < times2; i++)
                            {
                                flagGraphics.DrawString(_memory[j, i].ToString(), _drawFont, _drawBrush, 87 + 57 * j, 210 * l + NO_height + (i+1) * question_gap, _strFormat);
                            }
                            flagGraphics.DrawLine(_mPen, 30 + 57 * j, 1, 30 + 57 * j, 840);
                        }
                        else if (j == 1 || j == 3)//第2,4題是有負數的一位數六口
                        {
                            do
                            {
                                for (int i = 0; i < times1; i++)
                                {
                                    int random = _rnd.Next(1, (int)max1);
                                    _memory[j, i] = random;
                                }
                                int firstMinus = _rnd.Next(1, times1);
                                int secondMinus;
                                do
                                {
                                    secondMinus = _rnd.Next(1, times1);
                                } while (secondMinus == firstMinus);

                                _memory[j, firstMinus] *= -1;
                                _memory[j, secondMinus] *= -1;
                            } while (CheckForNegative(j, times1));

                            for (int i = 0; i < times1; i++)
                            {
                                flagGraphics.DrawString(_memory[j, i].ToString(), _drawFont, _drawBrush, 87 + 57 * j, 210 * l + NO_height + i * question_gap, _strFormat);
                            }
                            flagGraphics.DrawLine(_mPen, 30 + 57 * j, 1, 30 + 57 * j, 840);
                        }
                        else if (j == 6 || j == 8)//第7,9題是正的二位兩口+正的一位兩口+負的一位兩口
                        {
                            do
                            {
                                List<int> positions = new List<int> { 0, 1, 2, 3 };
                                for (int i = 0; i < times2; i++)
                                {
                                    if (positions.Count > 0)
                                    {
                                        int randomIndex = _rnd.Next(positions.Count);
                                        int position = positions[randomIndex];
                                        positions.RemoveAt(randomIndex);

                                        if (i < 2)
                                        {
                                            _memory[j, position] = _rnd.Next(1, (int)max1);
                                        }
                                        else
                                        {
                                            _memory[j, position] = _rnd.Next(10, (int)max2);
                                        }
                                    }
                                }

                                int firstMinus = _rnd.Next(0, times2);
                                int secondMinus;
                                do
                                {
                                    secondMinus = _rnd.Next(0, times2);
                                } while (secondMinus == firstMinus);

                                _memory[j, firstMinus] *= -1;
                                _memory[j, secondMinus] *= -1;
                            } while (CheckForNegative(j, times2));

                            for (int i = 0; i < times2; i++)
                            {
                                flagGraphics.DrawString(_memory[j, i].ToString(), _drawFont, _drawBrush, 87 + 57 * j, 210 * l + NO_height + (i+1) * question_gap, _strFormat);
                            }
                            flagGraphics.DrawLine(_mPen, 30 + 57 * j, 1, 30 + 57 * j, 840);
                        }
                    }
                }
            }
        }
        private void GenerateLevel9Questions()
        {
            toggleAnswerButton.Enabled = false;
            toggleAnswerButton.Text = "暫未開放";
            using (Graphics flagGraphics = Graphics.FromImage(_flag))
            {
                flagGraphics.Clear(Color.White);
                int times1 = 5; // 1-5題的口數
                int times2 = 4; // 6-10題的口數
                double max1 = 10; // 一位數的最大值
                double max2 = 100; // 二位數的最大值
                int NO_height = 30;
                //int question_gap = 31;

                DrawGrid();

                for (int l = 0; l < 4; l++)//4排題目
                {
                    for (int j = 0; j < 10; j++)//每排10題
                    {
                        int ans = 0;
                        int minus_check_ans = 0;
                        int minus_quest = 0;

                        if (j == 0 || j == 2 || j == 4)//第1,3,5題要是正的一位數五口
                        {
                            do
                            {
                                for (int i = 0; i < times1; i++)
                                {
                                    int random = _rnd.Next(1, (int)max1);
                                    _memory[j, i] = random;
                                }
                            } while (CheckForNegative(j, times1));

                            for (int i = 0; i < times1; i++)
                            {
                                flagGraphics.DrawString(_memory[j, i].ToString(), _drawFont, _drawBrush, 87 + 57 * j, 210 * l + NO_height + i * question_gap, _strFormat);
                            }
                            flagGraphics.DrawLine(_mPen, 30 + 57 * j, 1, 30 + 57 * j, 840);
                        }
                        else if (j == 5 || j == 7 || j == 9)//第6,8,10題是正的一位三口+二位一口
                        {
                            do
                            {
                                for (int i = 0; i < times2 ; i++)//只需要(4-1)三口數字 之後再把其中一口換成二位數
                                {
                                    int random = _rnd.Next(1, (int)max1);
                                    _memory[j, i] = random;
                                }
                                int specialNumber = _rnd.Next(0, 3);
                                _memory[j, specialNumber] = _rnd.Next(10, (int)max2);

                            } while (CheckForNegative(j, times2));

                            for (int i = 0; i < times2; i++)
                            {
                                flagGraphics.DrawString(_memory[j, i].ToString(), _drawFont, _drawBrush, 87 + 57 * j, 210 * l + NO_height + i * question_gap, _strFormat);
                            }
                            flagGraphics.DrawLine(_mPen, 30 + 57 * j, 1, 30 + 57 * j, 840);
                        }
                        else if (j == 1 || j == 3)//第2,4題是有負數的一位數五口
                        {
                            do
                            {
                                for (int i = 0; i < times1; i++)
                                {
                                    int random = _rnd.Next(1, (int)max1);
                                    _memory[j, i] = random;
                                }
                                int firstMinus = _rnd.Next(1, times1);
                                int secondMinus;
                                do
                                {
                                    secondMinus = _rnd.Next(1, times1);
                                } while (secondMinus == firstMinus);

                                _memory[j, firstMinus] *= -1;
                                _memory[j, secondMinus] *= -1;
                            } while (CheckForNegative(j, times1));

                            for (int i = 0; i < times1; i++)
                            {
                                flagGraphics.DrawString(_memory[j, i].ToString(), _drawFont, _drawBrush, 87 + 57 * j, 210 * l + NO_height + i * question_gap, _strFormat);
                            }
                            flagGraphics.DrawLine(_mPen, 30 + 57 * j, 1, 30 + 57 * j, 840);
                        }
                        else if (j == 6 || j == 8)//第7,9題是正的二位一口+正的一位三口+負的一位兩口
                        {
                            do
                            {
                                for (int i = 0; i < times2 ; i++)//只需要(4-1)三口數字 之後再把其中一口換成二位數 再把其中一口一位數換成負的
                                {
                                    int random = _rnd.Next(1, (int)max1);
                                    _memory[j, i] = random;
                                }
                                int specialNumber = _rnd.Next(0, 3);//二位數的位置
                                _memory[j, specialNumber] = _rnd.Next(10, (int)max2);

                                int firstMinus = _rnd.Next(1, times2);
                                int secondMinus;
                                do
                                {
                                    secondMinus = _rnd.Next(1, times2);
                                } while (secondMinus == firstMinus || secondMinus == specialNumber);

                                _memory[j, firstMinus] *= -1;
                                _memory[j, secondMinus] *= -1;
                            } while (CheckForNegative(j, times2));

                            for (int i = 0; i < times2; i++)
                            {
                                flagGraphics.DrawString(_memory[j, i].ToString(), _drawFont, _drawBrush, 87 + 57 * j, 210 * l + NO_height + i * question_gap, _strFormat);
                            }
                            flagGraphics.DrawLine(_mPen, 30 + 57 * j, 1, 30 + 57 * j, 840);
                        }
                    }
                }
            }
        }

        private void GenerateLevel10Questions()
        {
            toggleAnswerButton.Enabled = false;
            toggleAnswerButton.Text = "暫未開放";
            using (Graphics flagGraphics = Graphics.FromImage(_flag))
            {
                flagGraphics.Clear(Color.White);
                int times = 4;
                double max = 10;
                int NO_height = 30;
                int lastMinus = 0;
                int minus = 0;

                DrawGrid();

                for (int l = 0; l < 4; l++)//4排題目
                {
                    for (int j = 0; j < 10; j++)//每排10題
                    {
                        int ans = 0;
                        int minus_check_ans = 0;
                        int minus_quest = 0;

                        if (j == 0 || j == 2 || j == 4)//第1,3,5題要是正的一位數四口
                        {
                            for (int i = 0; i < times; i++)
                            {
                                int random = _rnd.Next(1, (int)max);
                                ans += random;
                                _memory[j, i] = random;
                                flagGraphics.DrawString(random.ToString(), _drawFont, _drawBrush, 87 + 57 * j, 210 * l + NO_height + i * 40, _strFormat);
                                flagGraphics.DrawLine(_mPen, 30 + 57 * j, 1, 30 + 57 * j, 840);
                            }
                        }
                        else if (j == 5 || j == 7 || j == 9)//第6,8,10題是正的一位二口+二位一口
                        {
                            for (int i = 0; i < times - 1; i++)//只需要(4-1)三口數字 之後再把其中一口換成二位數
                            {
                                int random = _rnd.Next(1, (int)max);
                                _memory[j, i] = random;
                                flagGraphics.DrawString(random.ToString(), _drawFont, _drawBrush, 87 + 57 * j, 210 * l + NO_height + i * 40, _strFormat);
                                flagGraphics.DrawLine(_mPen, 30 + 57 * j, 1, 30 + 57 * j, 840);
                            }
                            int specialNumber = _rnd.Next(0, 3);
                            _memory[j, specialNumber] = _rnd.Next(10, 100);

                            flagGraphics.FillRectangle(_whiteBrush, 87 + 57 * j - 23, 210 * l + NO_height + specialNumber * 40 + 1, 20, 20);//誤差1 故Y座標+1
                            flagGraphics.DrawString(_memory[j, specialNumber].ToString(), _drawFont, _drawBrush, 87 + 57 * j, 210 * l + NO_height + specialNumber * 40, _strFormat);
                        }
                        else if (j == 1 || j == 3)//第2,4題是有負數的一位數四口
                        {
                            for (int i = 0; i < times; i++)
                            {
                                int random = _rnd.Next(1, (int)max);
                                _memory[j, i] = random;

                                flagGraphics.DrawString(random.ToString(), _drawFont, _drawBrush, 87 + 57 * j, 210 * l + NO_height + i * 40, _strFormat);
                                flagGraphics.DrawLine(_mPen, 30 + 57 * j, 1, 30 + 57 * j, 840);
                            }

                            while (minus_quest < 1)
                            {
                            OneMoreTime:
                                minus = _rnd.Next(1, times);//負數的位置在(times)其中一(minus)口數(1~3之間)
                                minus_check_ans = 0;
                                _memory[j, minus] = _memory[j, minus] * -1;

                                for (int a = 0; a < times; a++)//驗算一下過程中有沒有負數
                                {
                                    minus_check_ans += _memory[j, a];
                                    if (minus_check_ans < 0)
                                    {
                                        _memory[j, minus] = _memory[j, minus] * -1;
                                        goto OneMoreTime;
                                    }
                                }

                                minus_quest++;
                                flagGraphics.FillRectangle(_whiteBrush, 87 + 57 * j - 23, 210 * l + NO_height + minus * 40, 20, 20);
                                flagGraphics.DrawString(_memory[j, minus].ToString(), _drawFont, _drawBrush, 87 + 57 * j, 210 * l + NO_height + minus * 40, _strFormat);
                                flagGraphics.DrawLine(_mPen, 30 + 57 * j, 1, 30 + 57 * j, 840);
                            }
                        }
                        else if (j == 6 || j == 8)//第7,9題是正的二位一口+正的一位一口+負的一位一口
                        {
                            for (int i = 0; i < times - 1; i++)//只需要(4-1)三口數字 之後再把其中一口換成二位數 再把其中一口一位數換成負的
                            {
                                int random = _rnd.Next(1, (int)max);
                                _memory[j, i] = random;
                                flagGraphics.DrawString(random.ToString(), _drawFont, _drawBrush, 87 + 57 * j, 210 * l + NO_height + i * 40, _strFormat);
                                flagGraphics.DrawLine(_mPen, 30 + 57 * j, 1, 30 + 57 * j, 840);
                            }
                            int specialNumber = _rnd.Next(0, 3);//二位數的位置
                            _memory[j, specialNumber] = _rnd.Next(10, 100);
                            flagGraphics.FillRectangle(_whiteBrush, 87 + 57 * j - 23, 210 * l + NO_height + specialNumber * 40 + 1, 20, 20);//誤差1 故Y座標+1
                            flagGraphics.DrawString(_memory[j, specialNumber].ToString(), _drawFont, _drawBrush, 87 + 57 * j, 210 * l + NO_height + specialNumber * 40, _strFormat);
                            int faultTimes=0;
                        OneMoreTime:
                            minus = _rnd.Next(1, 3);//負數的位置
                            if (minus == specialNumber)//負數不能是二位數或是在第一個數字(0)
                            {
                                goto OneMoreTime;//重新生成一次負數的位置
                            }
                            else
                            {
                                minus_check_ans = 0;
                                _memory[j, minus] = _memory[j, minus] * -1;
                                for (int i = 0; i < 3; i++)
                                {
                                    minus_check_ans += _memory[j, i];
                                    if (minus_check_ans < 0)
                                    {
                                        _memory[j, minus] = _memory[j, minus] * -1;
                                        faultTimes++;
                                        if (faultTimes > 100)
                                        {
                                            break;
                                        }
                                        goto OneMoreTime;
                                    }
                                }
                                if(faultTimes>100)
                                {
                                    int n0 = _memory[j, 0];
                                    _memory[j, 0] = _memory[j, specialNumber];
                                    _memory[j, 2] = n0*-1;

                                    flagGraphics.FillRectangle(_whiteBrush, 87 + 57 * j - 23, 210 * l + NO_height + 0 * 40 + 1, 20, 20);
                                    flagGraphics.FillRectangle(_whiteBrush, 87 + 57 * j - 26, 210 * l + NO_height + 2 * 40 + 1, 22, 20);
                                    flagGraphics.DrawString(_memory[j,0].ToString(), _drawFont, _drawBrush, 87 + 57 * j, 210 * l + NO_height + 0 * 40, _strFormat);
                                    flagGraphics.DrawString(_memory[j,2].ToString(), _drawFont, _drawBrush, 87 + 57 * j, 210 * l + NO_height + 2 * 40, _strFormat);
                                }
                            }
                            //_memory[j, minus] = _memory[j, minus] * -1;
                            flagGraphics.FillRectangle(_whiteBrush, 87 + 57 * j - 23, 210 * l + NO_height + minus * 40 + 1, 20, 20);
                            flagGraphics.DrawString(_memory[j, minus].ToString(), _drawFont, _drawBrush, 87 + 57 * j, 210 * l + NO_height + minus * 40, _strFormat);
                        }
                    }
                }
            }
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
