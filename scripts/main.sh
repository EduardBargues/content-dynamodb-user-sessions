set -e
WORKING_FOLDER=$(pwd)

cd $WORKING_FOLDER/src/Functions/CreateSession/src
    package="$WORKING_FOLDER/terraform/create-session.zip"
    dotnet lambda package \
        -c release \
        --output-package $package
cd $WORKING_FOLDER

# cd $WORKING_FOLDER/terraform
#     terraform init
#     terraform validate
#     planFile="terraform.plan"
#     terraform plan -out=$planFile
#     terraform apply $planFile
# cd $WORKING_FOLDER