pipeline {
    agent any
    options{
      ansiColor('xterm')
    }
        environment {
            registry = "https://hub.docker.com/r/yorgoc/userservice"
            registryCredential = 'dockerhub'
            dockerImage = ''
            scannerHome = tool 'sonarqube-scanner'
        }
    stages {
        stage('Initialize'){
            steps {
                
                   echo "PATH = ${PATH}"
                   echo "Running ${env.BUILD_ID} on ${env.JENKINS_URL}"
            }
        }
        stage('Clean') {
          steps {
            bat "msbuild.exe ${workspace}\\<path-to-solution\\<solution-project-name>.sln" /nologo /nr:false /p:platform=\"x64\" /p:configuration=\"release\" /t:clean"
          }
        }
        stage('Build') {
         steps {
          bat "msbuild.exe ${workspace}\\<path-to-solution>\\<solution-name>.sln /nologo /nr:false  /p:platform=\"x64\" /p:configuration=\"release\" /p:PackageCertificateKeyFile=<path-to-certificate-file>.pfx /t:clean;restore;rebuild"
         }
        }

         stage('Running unit tests') {
           steps {
             bat "dotnet add ${workspace}/<path-to-Unit-testing-project>/<name-of-unit-test-project>.csproj package JUnitTestLogger --version 1.1.0"
             bat "dotnet test ${workspace}/<path-to-Unit-testing-project>/<name-of-unit-test-project>.csproj --logger \"junit;LogFilePath=\"${WORKSPACE}\"/TestResults/1.0.0.\"${env.BUILD_NUMBER}\"/results.xml\" --configuration release --collect \"Code coverage\""
             powershell '''
               $destinationFolder = \"$env:WORKSPACE/TestResults\"
               if (!(Test-Path -path $destinationFolder)) {New-Item $destinationFolder -Type Directory}
               $file = Get-ChildItem -Path \"$env:WORKSPACE/<path-to-Unit-testing-project>/<name-of-unit-test-project>/TestResults/*/*.coverage\"
               $file | Rename-Item -NewName testcoverage.coverage
               $renamedFile = Get-ChildItem -Path \"$env:WORKSPACE/<path-to-Unit-testing-project>/<name-of-unit-test-project>/TestResults/*/*.coverage\"
               Copy-Item $renamedFile -Destination $destinationFolder
             '''            
           }        
         }
         stage('Convert coverage file to xml coverage file') {
           steps {
             bat "<path-to-CodeCoverage.exe>\\CodeCoverage.exe analyze  /output:${WORKSPACE}\\TestResults\\xmlresults.coveragexml  ${WORKSPACE}\\TestResults\\testcoverage.coverage"
           }
         }
        stage('Generate report') {
          steps {
              bat "<path-to-ReportGenerator.exe>\\ReportGenerator.exe -reports:${WORKSPACE}\\TestResults\\xmlresults.coveragexml -targetdir:${WORKSPACE}\\CodeCoverage_${env.BUILD_NUMBER}" 
          }
        } 

      stage('Sonar') {
            steps {
                withSonarQubeEnv('SonarQube') {
                    bat "${scannerHome}/bin/sonar-scanner"
                }
            }
        }
         stage('Quality Gate') {
            steps{
                timeout(time:1, unit: 'HOURS'){
                    waitForQualityGate abortPipeline: true
                }
            }
        }

        stage('Building image') {
          steps{
            script {
              dockerImage = docker.build("yorgoc/userservice:v1")
            }
          }
        }
        stage('Deploy Image') {
          steps{
            script {
                  docker.withRegistry('https://registry.hub.docker.com/r/yorgoc/userservice', registryCredential) {
                    dockerImage.push('v1')
                  }
            }
          }
        }
    }
}