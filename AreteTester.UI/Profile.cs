using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AreteTester.UI
{
    public class Profile
    {
        private const int MAX_RECENT_PROJECTS = 5;

        public string ProjectPath { get; set; }

        public string WebdriverLaunchUrl { get; set; }

        public List<string> AssistSelections { get; set; }

        public List<RecentProject> RecentProjects { get; set; }

        public Profile()
        {
            this.AssistSelections = new List<string>();
            this.RecentProjects = new List<RecentProject>();
        }

        public void AddRecentProject(string path)
        {
            bool pathExists = this.RecentProjects.Where(x => x.Path.ToLower() == path.ToLower()).Count() > 0;
            if (pathExists == false)
            {
                this.RecentProjects.Add(new RecentProject() { Path = path, DateTimeOpened = DateTime.Now });

                if (this.RecentProjects.Count > MAX_RECENT_PROJECTS)
                {
                    RecentProject oldestProject = this.RecentProjects.OrderBy(x => x.DateTimeOpened).First();
                    this.RecentProjects.Remove(oldestProject);
                }
            }
        }
    }

    public class RecentProject
    {
        public string Path { get; set; }

        public DateTime DateTimeOpened { get; set; }
    }
}
