# Classes

***
## Testing Data Classes
***

### SpatialTest - Composite
#### Attributes
* `datetime` dateTime
* `int` participantNumber
* `list` testBlocks

#### Methods
* setters and getters
* addBlock(block)

### TrialBlock - Composite
* `int` serialNumber (serialized)
* `list` trials (list of trials)
* `enum` orientation (horizontal/vetical)

#### Methods
* setters and getters
* generateTrials(orientation, number)

### SpatialTrial
#### Attributes
* `int` serialNumber (serialized)
* `enum` orientation (horizontal/vetical)
* `tuple` trialType (size : size)
* `int` accuracy (0 incorrect/1 correct)
* `int` responseTime (ms)

#### Methods
* setters and getters

### DotStimulii
* `enum` size

#### Methods
* getSize()

***
## Application Data Classes
***

### Application Settings

***
## Spreadsheet Classes
***

### CellData
#### Attributes
* `int` row
* `int` col
* `string` data

#### Methods
* `int` getRow()
* `int`getCol()
* `string` getData()

### SheetWriter
The class object will take in a list of CellData objects, along with 
relevant participant data, and write to a new sheet using the 
participant number as the name.

***
## Trial Classes
***
Classes relating to the drawing of the trials and their discrete
stages

### TrialFixation
#### Attributes
* `int` timeInterval

### TrialInterstimulus
#### Attributes
* `int` timeInterval

### TrialTargets
#### Attributes
* `int` timeInterval

### TrialMask
#### Attributes
* `int` timeInterval

### TrialFeedback
#### Attributes
* `int` timeInterval

### TrialIntertrial
#### Attributes
* `int` timeInterval