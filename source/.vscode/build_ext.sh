#!/bin/bash
echo "Building dev extensions..."

#   source path
working_dir=/home/badkraft/repos/razorware/gfxcore/source
dev_dir="$working_dir/dev"
debug="bin/Debug/net9.0"

#	extension names
resource=Resources.Bar
system=Systems.Foo

#   these two depend on gfxcore.domain
dotnet build $dev_dir/GfxCore.$resource/gfxcore.resources.bar.csproj
if [ $? -ne 0 ]; then
    echo "Build $resource failed."
    exit 1
else
    echo "Build $resource successful."

    #	create the extension directories
    mkdir -p $working_dir/mods/$resource
    
    #   now copy the bar & foo dlls into "mods" folder
    yes | cp -f -vi $dev_dir/GfxCore.$resource/$debug/gfxcore.resources.bar.dll $working_dir/mods/$resource
fi

dotnet build $dev_dir/GfxCore.$system/gfxcore.systems.foo.csproj
if [ $? -ne 0 ]; then
    echo "Build $system failed."
    exit 1
else
    echo "Build $system successful."

    #	create the extension directories
    mkdir -p $working_dir/mods/$resource

    #   now copy the bar & foo dlls into "mods" folder
    yes | cp -f -vi $dev_dir/GfxCore.$system/$debug/gfxcore.systems.foo.dll $working_dir/mods/$system
fi


