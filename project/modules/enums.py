from enum import Enum


class Orientation(Enum):
    HORIZONTAL = 0
    VERTICAL = 1


class TargetSize(Enum):
    XS = -2
    S = -1
    M = 0
    L = 1
    XL = 2