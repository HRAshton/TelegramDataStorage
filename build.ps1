$imageName = "telegram-data-storage-build"

docker build -f ./TestAndPublish.Dockerfile `
             --secret id=bot_token,src=.secrets.bot_token `
             --secret id=chat_id,src=.secrets.chat_id `
             --progress plain `
             --target result `
             -t $imageName `
             .

$id = $( docker create $imageName )
docker cp ${id}:/output/package/release/ ./artifacts/
docker rm $id
