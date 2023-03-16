# Unity CI Wizard
Unity Editor extension for generating CI configs from predefined templates.
Initially it supports only Gitlab CI, but written with future expansion in mind to support other CI systems.
CI scripts support automatic detection and installation of project's Unity version.

You can find the wizard in the "Window/CI" menu.

## Supported job types

### Semantic Version

Automatically increases project version. Which has to have format: {MAJOR}.{MINOR}.{PATCH}.
It will increase version only if the latest commit starts with one of predefined keywords:

- "mile:" - reset MINOR & PATCH to 0, increase MAJOR
- "feat:" - reset PATCH to 0, increase MINOR
- "fix:" - increase PATCH

Also it will increase Anroid/iOS build bundleVersionCode/buildNumber every time.

### Extract Android Keystore

Extracts keystore from $UCI_ANDROID_KEYSTORE CI variable (encoded in base64)

### Run Tests (Work in progress)

Run tests in project

### Build jobs

#### Android APK & AAB

These jobs have predefined settings to use keystore from "Extract Android Keystore" job.
Alternatively you can store your keystore in the repo and change job settings accordingly.

## Gitlab

Tested only with MacOs gitlab runners.

Unity installation is done via Unity Hub, so you have to install it on each runner.

By default jobs will run only on the main branch, you can specify multiple branches separated with '|' (main|develop)

You need to setup runner's environment variables (~/.gitlab-runner/config.toml):
> ...
> 
> environment = ["UCI_UNITY_HUB_PATH=/Applications/Unity Hub.app/Contents/MacOS/Unity Hub", "UCI_UNITY_EXECUTABLE=/Contents/MacOS/Unity"]
> 
> ...

### Gitlab project variables

#### License

- UCI_LICENSE_CREDENTIALS = -username {YOUR_UNITY_USERNAME} -password {YOUR_UNITY_PASSWORD}
- UCI_LICENSE_SERIAL = -serial {YOUR_UNITY_SERIAL}

#### Semantic version

Semantic version job commits updated ProjectSettings.asset via deploy key, so you need to setup one.

- CI_GIT_SSH_PRIVATE_KEY = {private key of your deploy key}
- CI_GIT_USER_EMAIL = {git user email}
- CI_GIT_USER_USERNAME = {git user username}
- CI_SERVER_SSH_PORT = {define only if your gitlab instance has custom ssh port}

#### Android Keystore

- UCI_ANDROID_KEYSTORE = {base64 encoded keystore file}
- UCI_ANDROID_KEYSTORE_ALIAS = {alias name}
- UCI_ANDROID_KEYSTORE_ALIAS_PASS = {alias password}
- UCI_ANDROID_KEYSTORE_PASS = {keystore password}