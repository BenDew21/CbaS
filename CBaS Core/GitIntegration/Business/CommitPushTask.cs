using System;
using System.ComponentModel;
using System.Diagnostics;
using CBaSCore.Framework.Business;
using CBaSCore.GitIntegration.Storage;
using LibGit2Sharp;
using Microsoft.Alm.Authentication;

namespace CBaSCore.GitIntegration.Business
{
    /// <summary>
    /// CommitPushTask - Task to commit and/or push the changes to Git
    /// </summary>
    public class CommitPushTask : BaseProgressAwareTask
    {
        /// <summary>
        /// Method run on a new thread
        /// </summary>
        /// <param name="sender">The sending task, usually this object</param>
        /// <param name="e">Event args</param>
        protected override void TaskDoWork(object sender, DoWorkEventArgs e)
        {
            // Pattern match cast parameters to useable types
            if (e.Argument is CommitPushDTO dto && sender is BaseProgressAwareTask task)
            {
                using (var repo = new Repository(dto.RepoPath))
                {
                    try
                    {
                        task.ReportProgress(-1, new ProgressChangedObject()
                        {
                            Message = "Committing...",
                            Action = (() => { GitHandler.GetInstance().LogToConsole("Committing..."); })
                        });
                        
                        // Stage the changes, and build the commit details
                        Commands.Stage(repo, dto.ChangedFiles);
                        var config = repo.Config;
                        var author = config.BuildSignature(DateTimeOffset.Now);

                        var commit = repo.Commit(dto.CommitMessage, author, author);
                        if (commit != null)
                        {
                            task.ReportProgress(-1, new ProgressChangedObject()
                            {
                                Message = "Commit Successful",
                                Action = (() => { GitHandler.GetInstance().LogToConsole("Author: " + commit.Author + " committed: " + commit.Message + " with SHA: " + commit.Sha); })
                            });

                            if (dto.ShouldPush)
                            {
                                task.ReportProgress(-1, new ProgressChangedObject()
                                {
                                    Message = "Pushing...",
                                    Action = (() => { GitHandler.GetInstance().LogToConsole("Pushing..."); })
                                });
                                
                                // Get HTTPS secrets for the system user
                                var secrets = new SecretStore("git");
                                var auth = new BasicAuthentication(secrets);
                                var creds = auth.GetCredentials(new TargetUri("https://github.com"));
                    
                                var remote = repo.Network.Remotes["origin"];
                                var options = new PushOptions();

                                options.CredentialsProvider = (url, fromUrl, types) => new UsernamePasswordCredentials
                                {
                                    Username = creds.Username,
                                    Password = creds.Password
                                };

                                // Create the push ref spec from the current branch head
                                var pushRefSpec = repo.Head.CanonicalName;
                                repo.Network.Push(remote, pushRefSpec, options);
                            
                                task.ReportProgress(-1, new ProgressChangedObject()
                                {
                                    Message = "Push Successful",
                                    Action = (() => { GitHandler.GetInstance().LogToConsole(commit.Sha + " pushed to remote on branch: " + repo.Head.CanonicalName); })
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                }
            }
        }
        
        /// <summary>
        /// Method called when the thread has finished
        /// </summary>
        /// <param name="sender">The sending task, usually this object</param>
        /// <param name="e">Event args</param>
        public override void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            GitHandler.GetInstance().UpdateChanges();
        }
    }
}