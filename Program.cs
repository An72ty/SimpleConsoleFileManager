using System;
using System.Data;
using System.Runtime.InteropServices;


namespace FileManager;

public class FileManager {
    string currentDirectory;
    string fileSaveCurrentDirectory;
    public FileManager(string file_save_current_directory) {
        fileSaveCurrentDirectory = file_save_current_directory;
        string path = "C:/";
        if (File.Exists(fileSaveCurrentDirectory)) {
            path = File.ReadAllText("info.txt");
        } else {
            File.WriteAllText("info.txt", "C:/");
        }
        currentDirectory = path;
    }

    public void Start() {
        bool is_quit = false;
        do {
            // Console.WriteLine($"{currentDirectory}\n{string.Join("/\n", Directory.GetDirectories(currentDirectory))}\n{string.Join("\n", Directory.GetFiles(currentDirectory))}");
            Console.WriteLine($"Current Directory - {currentDirectory} {Directory.GetCreationTime(currentDirectory)}");

            foreach (string directory in Directory.GetDirectories(currentDirectory)) {
                Console.WriteLine($"{directory}/ {Directory.GetCreationTime(directory)}");
            }

            foreach (string file in Directory.GetFiles(currentDirectory)) {
                Console.WriteLine($"{file} {new FileInfo(file).Length} {File.GetCreationTime(file)} {File.GetLastWriteTime(file)}");
            }
            
            string[] command_args = Console.ReadLine().Split(" ");
            // Console.WriteLine(string.Join("/", command_args));
            
            switch (command_args[0]) {
                case "q" :
                    is_quit = true;
                    if (File.Exists(fileSaveCurrentDirectory)) {
                        File.WriteAllText(fileSaveCurrentDirectory, currentDirectory);
                    }
                    break;
                case "cd":
                    // if (command_args[1].StartsWith('"') && command_args[1].EndsWith('"')) {
                    //     command_args[1].Trim('"');
                    // }

                    if (!Path.Exists(command_args[1]) && !Path.Exists(currentDirectory + command_args[1])) {
                        Console.WriteLine($"Directory {command_args[1]} doesn't exists");
                        break;
                    } else if (Path.Exists(command_args[1]) && command_args[1].EndsWith("/")) {
                        currentDirectory = command_args[1];
                    } else if (Path.Exists(command_args[1])) {
                        currentDirectory = command_args[1] + "/";
                    } else if (Path.Exists(currentDirectory + command_args[1]) && command_args[1].EndsWith("/")) {
                        currentDirectory += command_args[1];
                    } else if (Path.Exists(currentDirectory + command_args[1])) {
                        currentDirectory += command_args[1] + "/";
                    }

                    break;
            }
        } while (!is_quit);
        
    }
    private static long GetDirectorySize(string folderPath) {
        DirectoryInfo di = new DirectoryInfo(folderPath);
        return di.EnumerateFiles("*.*", SearchOption.TopDirectoryOnly).Sum(fi => fi.Length);
    }
}
    

public static class Program {
    public static void Main() {
        FileManager fileManager = new FileManager("info.txt");
        fileManager.Start();
    }
}