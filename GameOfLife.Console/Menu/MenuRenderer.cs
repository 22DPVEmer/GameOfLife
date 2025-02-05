using GameOfLife.Core.Constants;
public class MenuRenderer
{
    public void DisplayMainMenu()
    {
        System.Console.Clear();
        System.Console.WriteLine(DisplayConstants.WELCOME_TEXT);
        System.Console.WriteLine(DisplayConstants.SEPARATOR_LINE);
        System.Console.WriteLine(DisplayConstants.MENU_OPTION_1);
        System.Console.WriteLine(DisplayConstants.MENU_OPTION_2);
        System.Console.WriteLine(DisplayConstants.MENU_OPTION_3);
        System.Console.WriteLine(DisplayConstants.MENU_OPTION_4);
        System.Console.WriteLine(DisplayConstants.MENU_OPTION_5);
        System.Console.Write(DisplayConstants.MENU_PROMPT);
    }

    public void DisplaySaveFiles(IList<string> saves)
    {
        System.Console.WriteLine(DisplayConstants.AVAILABLE_SAVES_HEADER);
        for (int i = 0; i < saves.Count; i++)
        {
            System.Console.WriteLine(DisplayConstants.SAVE_FORMAT, i + 1, saves[i]);
        }
        System.Console.WriteLine(DisplayConstants.SAVE_SELECTION_PROMPT);
    }
} 