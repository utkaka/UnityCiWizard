# CI Wizard

An extension for the Unity editor to generate CI system configurations. **Currently implemented only for GitLab CI**, but can be extended to other CI systems as well.

It consists of several parts:
- A set of ScriptableObjects in the Unity project that are used to configure CI.
- A set of templates filled in using the [Scriban](https://github.com/scriban/scriban) library.
- Bash scripts for managing Unity versions, currently integrated into the templates themselves.

General working principle:
- The developer configures the desired CI system logic using ScriptableObjects.
- Templates are filled based on these objects, and the final CI system configuration is generated from them.
- When a CI build is triggered on the build server, bash scripts check via [Unity Hub CLI](https://docs.unity3d.com/hub/manual/HubCLI.html) whether the required Unity version is installed. If not, the installation is performed. If multiple CI processes on the same build server require the same version, only one process will install it, while the others wait.
- Next, it checks for the presence of the necessary Unity modules for the build. Their installation follows the same principle as the editor installation.
- A [workaround](https://discussions.unity.com/t/linux-editor-stuck-on-loading-because-of-bee_backend-w-workaround/854480/37) is applied for a problem that causes the Unity editor to hang on (some?) Linux systems. It is important to note that there may be multiple **bee_backend** files, and the workaround should be applied to all of them.
- The required version of Unity is launched, invoking the Execute method of the ScriptableObject corresponding to the job.

It should work on Linux and macOS. Currently tested and working on Ubuntu 22.04.

## Unity License

For proper operation, each build server must be assigned its own seat with a serial number, username, and password.

The license is activated when the job starts and is automatically returned after the job is completed. In case of any issues with the build server, this will allow it to be immediately replaced with another one.
## Installation

- Install [GitLab Runner](https://docs.gitlab.com/runner/install/) on a Linux or macOS server. **Important: for Unity Hub to work, Linux must (unfortunately) have a desktop environment installed. For example, it will not work on Ubuntu Server, you need Ubuntu Desktop.**
- Again, due to Unity Hub, the runner must be run in user mode.
- Install [Unity Hub](https://docs.unity3d.com/hub/manual/InstallHub.html).
- On Ubuntu 22.04 and possibly other Linux systems, you might encounter a [libssl issue](https://discussions.unity.com/t/workaround-for-libssl-issue-on-ubuntu-22-04/879165); the solution can be found [in this answer](https://discussions.unity.com/t/workaround-for-libssl-issue-on-ubuntu-22-04/879165/38).
- Set up environment variables (discussed below).
- Add this package to your project.
- Configure the desired CI behavior. You can use the pre-configured configurations from the samples in this package as an example.
- Generate .gitlab-ci.yml.
- Ensure that for all job tags, there are runners with the corresponding tags.
- Commit the changes and monitor the build status.

## Environment Variables

It's better to set them [in the runner configuration itself](https://docs.gitlab.com/ee/ci/variables/#define-a-cicd-variable-in-the-ui), as these parameters are specific to each individual, but you can also set them [in GitLab itself](https://docs.gitlab.com/ee/ci/variables/#define-a-cicd-variable-in-the-ui).

- **UCI_ENV_UNITY_USERNAME** - the username for license activation
- **UCI_ENV_UNITY_PASSWORD** - the password
- **UCI_ENV_UNITY_SERIAL** - the serial number
- **UCI_ENV_UNITY_HUB_COMMAND** - the command to launch Unity Hub or the path to the executable ("/Applications/Unity Hub.app/Contents/MacOS/Unity Hub" on macOS, "unityhub" on linux)
- **UCI_ENV_UNITY_EXECUTABLE** - the path to the Unity editor executable, relative to its directory ("/Contents/MacOS/Unity" on macOS, "" on linux)

## Setup

First, you need to create a CiConfig asset (Create/Ci/Config), into which you should then add all the necessary jobs. After that, use the 'Overwrite .gitlab-ci' button to generate a .gitlab-ci.yml file with everything needed for their execution.

### **Main Parameters**

- **When** - the [condition](https://docs.gitlab.com/ee/ci/yaml/#when) for executing this job.
- **Branches** - the branches where this job should be added to the pipeline. Multiple branches can be specified using the "|" separator, for example, "main|develop".
- **Cache** - a separate asset listing the folders that need to be cached. Typically, it makes sense to cache the "Library" folder and "DoNotShip" folders for each build target. However, cache extraction and writing can be resource-intensive operations. For large projects, it may be worth updating the cache only in rarely executed pipelines (e.g., on the release or main branches), and for daily builds, only download the cache. For small projects, you can forgo caching altogether. It’s important to note that GitLab has a rather strange limitation of [4 caches per project](https://docs.gitlab.com/ee/ci/yaml/#cache).
- **[Cache policy](https://docs.gitlab.com/ee/ci/yaml/#cachepolicy)**
- **Artifacts** - [artifacts](https://docs.gitlab.com/ee/ci/yaml/#artifacts) that will be available to all jobs in subsequent pipeline stages, as well as for download in GitLab.

### Semantic Version

Executed in the first stage of the pipeline. It does not require the editor, as it is entirely implemented in bash. For correct operation, the Unity project version must be in the format 'X.YY.ZZ'. The script then checks the commit message and description. If either starts with certain keywords, the version is incremented. The Android bundleVersionCode is also incremented.

- "mile:" - resets YY & ZZ to 00, increments X
- "feat:" - resets ZZ to 00, increments YY
- "fix:" - increments ZZ

The updated `ProjectSettings.asset` is passed to the build jobs through artifacts. A commit and tag are also created in Git.

Environment variables that should be set in GitLab:

- CI_GIT_SSH_PRIVATE_KEY = private key of [your deploy key](https://docs.gitlab.com/ee/user/project/deploy_keys/)
- CI_GIT_USER_EMAIL = git user email
- CI_GIT_USER_USERNAME = git user username

### Build jobs

You only need to specify the name of the executable file. It is also possible to create your own classes for preprocessing and postprocessing, inheriting from BuildScriptablePreprocessor and BuildScriptablePostprocessor, respectively. You can define any desired logic in these classes.

### Android build jobs

Additionally, Android build configurations specify keystore parameters.

For convenience, you can specify these in GitLab environment variables:

- **UCI_ENV_ANDROID_KEYSTORE** - base64 encoded keystore file
- **UCI_ENV_ANDROID_KEYSTORE_PASS** - keystore password
- **UCI_ENV_ANDROID_KEYSTORE_ALIAS** - alias
- **UCI_ENV_ANDROID_KEYSTORE_ALIAS_PASS** - alias password

In this case, the keyStore parameter in the job configuration should be left empty.