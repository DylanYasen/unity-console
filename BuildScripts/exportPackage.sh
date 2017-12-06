#! /bin/sh

project_path=$(pwd)
library_name=uConsole

error_code=0

echo "Creating package."
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
  -batchmode \
  -nographics \
  -silent-crashes \
  -projectPath $project_path \
  -logFile "log/packaging.log" \
  -exportPackage "Assets/$library_name" "build/$library_name.unitypackage" \
  -quit
if [ $? = 0 ] ; then
  echo "Created package successfully."
  error_code=0
else
  echo "Creating package failed. Exited with $?."
  error_code=1
fi

echo 'Build logs:'
cat "$project_path/log/packaging.log" 

echo "Finishing with code $error_code"
exit $error_code
