def PLC(mOcc):
    #PLC: determines switch positions and transit light states using boolean operations on block occupancies of green line
    #<[]mOcc> bool containing occupancy of a block from green 58 to green 143, 
    #   where 1 represents a block that is either occupied or under maintenance. 
    #   Array index = Block no - 58
    #<[]mSwitches> bool containing position of switch where 1 represents left. 
    #   Switch 0 = Block 63, Switch 1 = Block 77, Switch 2 = Block 85
    #<[]mRightLights> bool containing state of right transit lights where 1 represents a green light
    #<[]mLeftLights> bool containing state of left transit lights where 1 represents a green light
    
    mSwitches = [False]*3
    mRightLights = [False]*len(mOcc)
    mLeftLights = [False]*9
    
    #Switch logic
    mSwitches[0] = mOcc[4]
    mSwitches[1] = mOcc[19] or mOcc[20] or mOcc[21] or mOcc[22] or mOcc[23] or mOcc[24] or mOcc[25] or mOcc[26] or mOcc[27]
    mSwitches[2] = mOcc[19] or mOcc[20] or mOcc[21] or mOcc[22] or mOcc[23] or mOcc[24] or mOcc[25] or mOcc[26] or mOcc[27]

    #In general, right light is used to enter higher block number, left light is used to enter lower block number
    for i in range(0,86):
        if i < 85:
            mRightLights[i] = not(mOcc[i + 1])
        if i > 19 and i <=27:
            mLeftLights[i - 19] = not(mOcc[i - 1])

    #All blocks in section N must be empty in order to enter two-way section
    mRightLights[42] = not(mOcc[19] or mOcc[20] or mOcc[21] or mOcc[22] or mOcc[23] or mOcc[24] or mOcc[25] or mOcc[26] or mOcc[27])
    mRightLights[18] = not(mOcc[19] or mOcc[20] or mOcc[21] or mOcc[22] or mOcc[23] or mOcc[24] or mOcc[25] or mOcc[26] or mOcc[27])
    
    #Moving across switch from N to R
    mLeftLights[0] = not(mOcc[43])
    return mSwitches, mRightLights, mLeftLights
