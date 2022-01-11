set -e
WORKING_FOLDER=$(pwd)

FEATURES=('CreateSession' 'GetSessionByToken')
 
for feature in "${FEATURES[@]}"
do
    echo ""
    echo "====> PACKAGING LAMBDA $feature"
    cd "$WORKING_FOLDER/src/Features/$feature"
    package="$WORKING_FOLDER/terraform/$feature.zip"
    dotnet lambda package \
        -c release \
        --output-package $package
    cd $WORKING_FOLDER
done

cd $WORKING_FOLDER/terraform
    echo ""
    echo "====> TERRAFORM"
    terraform init
    terraform validate
    planFile="terraform.plan"
    terraform plan -out=$planFile
    terraform apply $planFile
cd $WORKING_FOLDER