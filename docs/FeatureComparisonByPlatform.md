# PlatformKit Feature Comparison
This document is not guaranteed to be complete, accurate, or up to date.

## Feature Comparison Table Symbol Legend
| Symbol | Use/Meaning |
|-|-|
| Supported | :heavy_check_mark: |
| Unsupported | :x: | 
| Deprecated | :warning: |
| Removed | :no_entry: |
| Being Developed | :construction: |
| Coming Soon | :soon: |
| TBD | :date: |


## Table of Contents

Features:
1) [OS Detection](#os-detection)
2) [Process and Command Execution](#process-and-command-execution)

## Features

### OS Detection
| | Windows | macOS | Linux | FreeBSD |
|-|-|-|-|-|
| OS Name | :heavy_check_mark: | :heavy_check_mark:, Added in 2.5.0 Alpha 5 | :heavy_check_mark:, Added in 2.0.0 Beta 4 | :x: |
| OS Version | :heavy_check_mark: | :heavy_check_mark:, Added in 2.5.0 Alpha 3 | :heavy_check_mark:, Added in 2.0.0 Beta 4. | :heavy_check_mark:, Added in 3.0.0 Alpha 2 |
| OS Build Number/Build String | :heavy_check_mark: | :heavy_check_mark:, Added in 2.5.0 Alpha 5 | N/A | :x: |
| OS Edition | :heavy_check_mark:, Added in 2.5.0 Alpha 6 | N/A | N/A | N/A |

### Process and Command Execution
| | Windows | macOS | Linux | FreeBSD |
|-|-|-|-|-|
| RunProcess (Generic) | ✔️, 2.0.0 | ✔️, 2.0.0 | ✔️, 2.0.0 | :x: | 
| RunProcess___ | ✔️, 2.0.0 | ✔️, 2.0.0 | ✔️, 2.0.0 | :x: |
| Run___Command | :heavy_check_mark:, Added in 2.4.0 | :heavy_check_mark:, Added in 2.4.0 | :heavy_check_mark:, Added in 2.4.0 | :x: |
| RunCommand___ | :no_entry:, Removed in 3.0.0 | :no_entry:, Removed in 3.0.0 | :no_entry:, Removed in 3.0.0 | :x: |
| RunCommand (Generic) | :no_entry: , Removed in 3.0.0 | :no_entry:, Removed in 3.0.0 | :no_entry:, Removed in 3.0.0 | :x: |
