### README

# 珠心算出題_加減_

This project is a Windows Forms application for generating arithmetic questions for abacus mental arithmetic practice. The application supports different difficulty levels and can generate questions with specified conditions.

## Features

- Generate arithmetic questions for different levels (1-11).
- Customize question display with different fonts and sizes.
- Display or hide answers.
- Save generated questions as an image.

## Requirements

- .NET Framework 4.7.2 or later.
- Windows operating system.

## How to Use

1. **Run the Application**
   - Open the solution in Visual Studio.
   - Build and run the project.

2. **Select Difficulty Level**
   - Use the combo box to select the desired difficulty level (1-11).

3. **Customize Display**
   - Choose the font and size for displaying questions.
   - Adjust the question gap using the numeric up/down controls.

4. **Generate Questions**
   - Click the "Generate Questions" button to create questions based on the selected difficulty level.

5. **Show/Hide Answers**
   - Use the "Toggle Answer" button to display or hide the answers.

6. **Save Questions as Image**
   - Click the "Save" button to save the generated questions as an image file.

## Function Descriptions

### `Form1`

- **Constructor**: Initializes the form and its components, including the difficulty combo box and toggle answer button.

### `InitializeComboBox()`

- Initializes the difficulty selection combo box and sets up event handling for changes in selection.

### `DifficultyComboBox_SelectedIndexChanged()`

- Adjusts the question gap based on the selected difficulty level.

### `InitializeToggleAnswerButton()`

- Sets up the toggle answer button for showing/hiding answers.

### `ToggleAnswerButton_Click()`

- Toggles the display of answers and updates the button text accordingly.

### `ShowOrHideAnswers()`

- Shows or hides answers based on the current state of the `showAnswers` variable.

### `CheckForNegative()`

- Checks if the calculation process for a given question results in negative numbers.

### `button1_Click()`

- Generates questions based on the selected difficulty level and displays them.

### `GenerateQuestions()`

- Switches between different question generation methods based on the selected difficulty level.

### `GenerateLevelXQuestions()`

- Generates questions for a specific difficulty level (1-11).

### `button2_Click()`

- Saves the generated questions as an image file.

### `button3_Click()`

- Saves the generated questions as an image file to a specific location.

## 文件描述

### `Form1`

- **構造函數**: 初始化表單及其組件，包括難度選擇框和切換答案按鈕。

### `InitializeComboBox()`

- 初始化難度選擇框並設置選擇更改的事件處理。

### `DifficultyComboBox_SelectedIndexChanged()`

- 根據選擇的難度級別調整題目間距。

### `InitializeToggleAnswerButton()`

- 設置切換答案按鈕，用於顯示/隱藏答案。

### `ToggleAnswerButton_Click()`

- 切換答案的顯示，並相應更新按鈕文本。

### `ShowOrHideAnswers()`

- 根據當前的 `showAnswers` 變量狀態顯示或隱藏答案。

### `CheckForNegative()`

- 檢查給定問題的計算過程是否產生負數。

### `button1_Click()`

- 基於選擇的難度級別生成問題並顯示。

### `GenerateQuestions()`

- 根據選擇的難度級別切換不同的問題生成方法。

### `GenerateLevelXQuestions()`

- 生成特定難度級別（1-11）的問題。

### `button2_Click()`

- 將生成的問題保存為圖像文件。

### `button3_Click()`

- 將生成的問題保存為圖像文件到特定位置。

## 計畫文件夾結構

- `Properties/`: 包含應用程序的屬性設置。
- `Resources/`: 包含應用程序使用的資源。
- `Form1.cs`: 主表單文件，包含應用程序的邏輯和功能。
- `Program.cs`: 應用程序的入口點。

## 授權

此項目使用 MIT 許可證 - 詳情請參閱 `LICENSE` 文件。

## 聯繫方式

如果有任何問題或建議，請聯繫：

- 電子郵件：memory509609@gmail.com
- GitHub: https://github.com/ice175848

---

This README provides a comprehensive guide on how to use the arithmetic question generation application. For any further assistance, please refer to the contact information provided above.
