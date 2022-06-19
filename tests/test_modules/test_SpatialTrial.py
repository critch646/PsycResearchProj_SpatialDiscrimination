"""
Contains unit tests for the Spatial Trial class.
"""

# Local Application Imports
from project.modules.SpatialTrial import SpatialTrial
from project.modules.enums import Orientation, TargetSize


def example_spatial_trial_object():
    """
    Example SpatialTrial object for testing

    :return: SpatialTest object
    """
    return SpatialTrial(1, Orientation.VERTICAL, (TargetSize.M, TargetSize.L))


def test_init_serial_number():
    """
    Tests init serial number parameter.

    :return: bool
    """

    test_obj = example_spatial_trial_object()

    assert test_obj.get_serial_number() == 1


def test_init_orientation():
    """
    Tests init orientation parameter.

    :return: bool
    """

    test_obj = example_spatial_trial_object()

    assert test_obj.get_orientation() == Orientation.VERTICAL


def test_init_trial_type():
    """
    Tests init trial type parameter.

    :return: bool
    """

    test_obj = example_spatial_trial_object()

    assert test_obj.get_trial_type() == (TargetSize.M, TargetSize.L)


def test_set_accuracy0():
    """
    Tests setting of accuracy with a 0.

    :return: bool
    """

    test_obj = example_spatial_trial_object()

    test_obj.set_accuracy(0)

    assert test_obj.get_accuracy() == 0


def test_set_accuracy1():
    """
    Tests setting of accuracy with a 1.

    :return: bool
    """

    test_obj = example_spatial_trial_object()

    test_obj.set_accuracy(1)

    assert test_obj.get_accuracy() == 1


def test_set_response_time():
    """
    Tests setting of response time.

    :return: bool
    """

    test_obj = example_spatial_trial_object()

    test_obj.set_response_time(1)

    assert test_obj.get_response_time() == 1