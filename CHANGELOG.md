# Changelog  

## 1.0.4
 - Apparently the package has had the last version's dll this whole time, which does not work.
	- Updated package to include a working dll
 - Recompiled on latest version of the game (issue has still not been fixed lmao)

## 1.0.3
 - Updated hook from Awake to PreAwake for latest update (feb17th)

## 1.0.2
 - packaging correct dll, 1.0.1 was a repackage of 1.0.0

## 1.0.1
 - Fixed compatibility with mods that rely on the DugeonTasks instance being assigned at Awake. (ContractQueen)
	- Changed patching to be much less aggresive in what it changes, only skipping DungeonTasks.CreateTasks when it has been called too early.
 - Added license

## 1.0.0  
 - Initial mod upload, I don't expect to have to update this ever again.  
