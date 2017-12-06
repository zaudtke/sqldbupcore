#!/bin/bash

#need to run chmod =x ./newmigration.sh to setup

version=$1
name=$2
filedate=$(date -u +%Y%m%d%H%M%S)

cleanname="${name// /_}"

filepath="migrations/${version}/${filedate}_${cleanname}.sql"


echo "/* Migration Script */" > $filepath
echo "$filepath created" 