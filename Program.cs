using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;


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
            Console.WriteLine($"Current Directory - {currentDirectory} {Directory.GetCreationTime(currentDirectory)} {GetDirectorySize(currentDirectory)}");

            foreach (string directory in Directory.GetDirectories(currentDirectory)) {
                Console.WriteLine($"{directory}/ {Directory.GetCreationTime(directory)} {GetDirectorySize(directory)}");
            }

            foreach (string file in Directory.GetFiles(currentDirectory)) {
                Console.WriteLine($"{file} {new FileInfo(file).Length} {File.GetCreationTime(file)} {File.GetLastWriteTime(file)}");
            }
            
            string[] command_args = Console.ReadLine().Split(" ");

            
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

                    CD(command_args);

                    break;
                case "del":
                    Delete(command_args[1]);

                    break;
                case "copy":
                    Copy(command_args[1]);
                    
                    break;

            }
        } while (!is_quit);
        
    }

    private string CD(string[] command_args) {
        // if (!Path.Exists(command_args[1]) && !Path.Exists(currentDirectory + command_args[1])) {
        //     Console.WriteLine($"Directory {command_args[1]} doesn't exists");
        //     return "Error";
        // } else if (Path.Exists(command_args[1]) && command_args[1].EndsWith("/")) {
        //     currentDirectory = command_args[1];
        // } else if (Path.Exists(command_args[1])) {
        //     currentDirectory = command_args[1] + "/";
        // } else if (Path.Exists(currentDirectory + command_args[1]) && command_args[1].EndsWith("/")) {
        //     currentDirectory += command_args[1];
        // } else if (Path.Exists(currentDirectory + command_args[1])) {
        //     currentDirectory += command_args[1] + "/";
        // }
        string full_path = GetFullPath(command_args[1]);
        if (full_path == "Error") {
            return full_path;
        }
        currentDirectory = full_path;

        return currentDirectory;
    }

    private string Delete(string path) {
        string full_path = GetFullPath(path);
        if (full_path == "Error") {
            return full_path;
        }
        if (File.Exists(full_path.TrimEnd('/'))) {
            File.Delete(full_path.TrimEnd('/'));
        } else if (Directory.Exists(full_path)) {
            Directory.Delete(full_path);
        } else {
            Console.WriteLine($"Directory {path} doesn't exists1");
        }

        return full_path;
    }

    private string Copy(string source_path) {
        string full_source_path = GetFullPath(source_path).TrimEnd('/');
        if (full_source_path == "Error" || !File.Exists(full_source_path.TrimEnd('/'))) {
            return full_source_path;
        }
        File.Copy(source_path, currentDirectory + source_path.Split("/").Last(), true);
        return full_source_path.TrimEnd();
    }

    private string GetFullPath(string path) {
        string full_path = "Error";
        if (!Path.Exists(path) && !Path.Exists(currentDirectory + path)) {
            Console.WriteLine($"Directory {path} doesn't exists2");
            return "Error";
        } else if (Path.Exists(path) && path.EndsWith("/")) {
            full_path = path;
        } else if (Path.Exists(path)) {
            full_path = path + "/";
        } else if (Path.Exists(currentDirectory + path) && path.EndsWith("/")) {
            full_path = currentDirectory + path;
        } else if (Path.Exists(currentDirectory + path)) {
            full_path = currentDirectory + path + "/";
        }
        
        return full_path;
    }

    private string GetDirectorySize(string folderPath) {
        try {
            DirectoryInfo di = new DirectoryInfo(folderPath);
            return Convert.ToString(di.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length));
        } catch (Exception exception) {
            return "Error";
        }
    }
}
    

public static class Program {
    public static void Main() {
        FileManager fileManager = new FileManager("info.txt");
        fileManager.Start();
    }
}