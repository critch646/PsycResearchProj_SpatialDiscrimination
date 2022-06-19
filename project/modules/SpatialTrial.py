from .enums import TargetSize, Orientation


class SpatialTrial:

    __serial_number = None
    __orientation = None
    __trial_type = None
    __accuracy = None
    __response_time = None

    def __init__(self, serial_number: int, orientation: Orientation, trial_type: tuple[TargetSize, TargetSize]):
        self.__serial_number = serial_number
        self.__orientation = orientation
        self.__trial_type = trial_type

    def get_serial_number(self) -> int:
        return self.__serial_number

    def get_orientation(self) -> Orientation:
        return self.__orientation

    def get_trial_type(self) -> tuple[TargetSize, TargetSize]:
        return self.__trial_type

    def get_accuracy(self) -> int:
        return self.__accuracy

    def get_response_time(self) -> int:
        return self.__response_time

    def set_accuracy(self, value: int):
        self.__accuracy = value

    def set_response_time(self, value: int):
        self.__response_time = value
