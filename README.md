## How to use this repo

1. **Download the Repository**

   - Click on the green "Code" button at the top right of the repository page.
   - Select "Download ZIP".

2. **Unzip the File**

   - Locate the downloaded ZIP file on your computer.
   - Right-click the file and select "Extract All..." to unzip it.

3. **Open the Folder in Visual Studio Code**

   - Open Visual Studio Code.
   - Click on "File" > "Open Folder..." and navigate to the unzipped folder `ZHAW_CAS-main`.

4. **Create a Python Environment**

   - Press `Ctrl+P` to open the command palette.
   - Type `Python: Create Environment` and select it.
   - Choose `Conda` as the environment type.

5. **Activate the Environment**

   - Open the terminal in Visual Studio Code by selecting `Terminal` > `New Terminal`.
   - Activate the environment by running the command:
     ```bash
     conda activate ./.conda
     ```

6. **Install Required Packages**

   - In the terminal, navigate to the project directory if not already there.
   - Run the following command to install the required packages:
     ```bash
     pip install -r requirements.txt
     ```

7. **Run the Project**

   - Follow any additional instructions provided in the repository to run the project.

8. **Additional Resources**
   - Refer to the documentation or comments within the code for further guidance.
