using DotNetEnv;

namespace Usuarios.WebAPI
{
    public static class EnvLoader
    {
        public static void Load()
        {
            var envPath = FindEnvFile(Directory.GetCurrentDirectory());

            if (envPath != null)
            {
                Env.Load(envPath);
            }
        }

        private static string? FindEnvFile(string startPath)
        {
            var currentPath = startPath;

            for (int i = 0; i < 6; i++)
            {
                var envFile = Path.Combine(currentPath, ".env");
                if (File.Exists(envFile))
                {
                    return envFile;
                }

                var parentDir = Directory.GetParent(currentPath);
                if (parentDir == null) break;
                currentPath = parentDir.FullName;
            }

            return null;
        }
    }
}
