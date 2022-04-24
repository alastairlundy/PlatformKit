## PlatformKit Feature Comparison by Platform 

### OS Detection
| | Windows | macOS | Linux | FreeBSD |
|-|-|-|-|-|
| OS Name | | :heavy_check_mark:, introduced in 2.5.0| :heavy_check_mark:, introduced in | :x: |
| OS Version | | :heavy_check_mark:, introduced in 2.5.0 | | :x: |
| OS Build Number/Build String | | :heavy_check_mark:, introduced in 2.5.0 | :x: | :x: |
| OS Edition | :heavy_check_mark:, introduced in 2.5.0 | N/A | N/A | N/A |

### Process and Command Execution
| | Windows | macOS | Linux | FreeBSD |
|-|-|-|-|-|
| RunProcess (Generic) | | | | | 
| RunProcess___ | | | 2nd Iteration: Introduced in 2.4.0 | :x: |
| Run___Command | :heavy_check_mark:, Introduced in 2.4.0 | :heavy_check_mark:, Introduced in 2.4.0 | :heavy_check_mark:, Introduced in 2.4.0 | :x: |
| RunCommand___ | :warning:, Deprecated in 2.4.0 | :warning:, Deprecated in 2.4.0 | :warning:, Deprecated in 2.4.0 | :x: |
| RunCommand (Generic) | :warning:, Deprecated in 2.4.0 | :warning:, Deprecated in 2.4.0 | :warning:, Deprecated in 2.4.0 | :x: |
