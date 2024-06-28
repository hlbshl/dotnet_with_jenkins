pipeline {
    agent any
    parameters {
        string(name: 'BROWSER', defaultValue: 'Chrome', description: 'Browser to run UI tests (for manual trigger)')
    }
    triggers {
        cron('H 0 * * *')
    }
    environment {
        GIT_CREDENTIALS_ID = 'GitLab_Token'
        GIT_BRANCH = 'main'
    }

    stages {
        stage('Checkout') {
            steps {
                script {
                    echo 'Checking out the repository'
                    git branch: env.GIT_BRANCH, credentialsId: env.GIT_CREDENTIALS_ID, url: 'https://git.epam.com/hleb_shulhin/practicaltaskselenium.git' }
            }
        }
        stage('Build') {
            steps {
                script {
                    echo 'Building the solution'
                    bat 'dotnet build PracticalTaskSelenium.sln'
                }
            }
        }
        stage('Run API tests') {
            steps {
                script {
                    try {
                        echo 'Running API tests'
                        bat '  dotnet test --filter Category=API --logger:"nunit;LogFilePath=TestResult_API.xml"'
                    } catch (Exception e) {
                        echo 'API tests failed'
                    }
                }
            }
        }
        stage('Post-process API test results') {
            steps {
                script {
                    echo 'Post-processing API test results'
                    bat '''
                echo ^<?xml version="1.0" encoding="UTF-8"?^> > temp.xml
                type TestResult_API.xml >> temp.xml
                move /Y temp.xml TestResult_API.xml
            '''
                }
            }
        }
        stage('Run UI tests') {
            steps {
                script {
                    try {
                        def browserType = ''
                        if (currentBuild.getBuildCauses('hudson.model.Cause$UserIdCause').size() > 0) {
                            echo "Manual trigger detected. Using browser: ${params.BROWSER}"
                            browserType = params.BROWSER
                        } else {
                            echo 'Scheduled or merge request trigger detected. Reading browser type from config'
                            def configFile = readFile('EnvironmentSettings.json')
                            def configJson = new groovy.json.JsonSlurperClassic().parseText(configFile)
                            browserType = configJson.TestEnvironment.BrowserType
                        }

                        def configFile = readFile('EnvironmentSettings.json')
                        def configJson = new groovy.json.JsonSlurperClassic().parseText(configFile)
                        configJson.TestEnvironment.BrowserType = browserType
                        configJson.TestEnvironment.Headless = true
                        writeFile file: 'EnvironmentSettings.json', text: groovy.json.JsonOutput.toJson(configJson)

                        echo 'Running UI tests'
                        bat 'dotnet test --filter Category=UI --logger "nunit;LogFilePath=TestResult_UI.xml"'
                    } catch (Exception e) {
                        echo 'UI tests failed'
                    }
                }
            }
        }
        stage('Post-process UI test results') {
            steps {
                script {
                    echo 'Post-processing UI test results'
                    bat '''
                echo ^<?xml version="1.0" encoding="UTF-8"?^> > temp.xml
                type TestResult_UI.xml >> temp.xml
                move /Y temp.xml TestResult_UI.xml
            '''
                }
            }
        }
        stage('Publish test results and artifacts') {
            steps {
                script {
                    echo 'Publishing test results and artifacts'
                    try {
                        nunit testResultsPattern: '**/TestResult_*.xml'
                    } catch (Exception e) {
                        echo 'Failed to publish test results'
                    }
                    archiveArtifacts artifacts: '**/TestResult_*.xml, **/*.log, **/Display_*.png', fingerprint: true
                }
            }
        }
    }
}