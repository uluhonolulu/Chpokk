COMPILE_TARGET = ENV['config'].nil? ? "Debug" : ENV['config'] # Keep this in sync w/ VS settings since Mono is case-sensitive
CLR_TOOLS_VERSION = "v4.0.30319"

buildsupportfiles = Dir["#{File.dirname(__FILE__)}/buildsupport/*.rb"]
raise "Run `git submodule update --init` to populate your buildsupport folder." unless buildsupportfiles.any?
buildsupportfiles.each { |ext| load ext }
solution_dir = "src"

include FileTest
#require 'albacore'
#load "VERSION.txt"

RESULTS_DIR = "results"
PRODUCT = "FubuMVC.RippleDemo"
COPYRIGHT = 'Copyright 2011 Ian Battersby. All rights reserved.';
COMMON_ASSEMBLY_INFO = 'src/CommonAssemblyInfo.cs';
BUILD_DIR = File.expand_path("build")
ARTIFACTS = File.expand_path("artifacts")

@teamcity_build_id = "bt99"
tc_build_number = ENV["BUILD_NUMBER"]
build_revision = tc_build_number || Time.new.strftime('5%H%M')
BUILD_NUMBER = "0.1"

props = { :stage => BUILD_DIR, :artifacts => ARTIFACTS }

desc "**Default**, compiles"
task :default => [:compile]

desc "Prepares the working directory for a new build"
task :clean do
	#TODO: do any other tasks required to clean/prepare the working directory
	FileUtils.rm_rf props[:stage]
    # work around nasty latency issue where folder still exists for a short while after it is removed
    waitfor { !exists?(props[:stage]) }
	Dir.mkdir props[:stage]
    
	Dir.mkdir props[:artifacts] unless exists?(props[:artifacts])
end

def waitfor(&block)
  checks = 0
  until block.call || checks >10 
    sleep 0.5
    checks += 1
  end
  raise 'waitfor timeout expired' if checks > 10
end

desc "Compile demo"
task :compile => [:clean] do  
		MSBuildRunner.compile :compilemode => COMPILE_TARGET, :solutionfile => "#{solution_dir}/AppHarbor.sln", :clrversion => CLR_TOOLS_VERSION
end

desc "Opens the Serenity Jasmine Runner in interactive mode"
task :open_jasmine do
	serenity "jasmine interactive src/serenity.txt"
end

desc "Runs the Jasmine tests"
task :run_jasmine do
	serenity "jasmine run src/serenity.txt"
end

def self.serenity(args)
  serenity = Platform.runtime(Nuget.tool("Serenity", "SerenityRunner.exe"))
  sh "#{serenity} #{args}"
end