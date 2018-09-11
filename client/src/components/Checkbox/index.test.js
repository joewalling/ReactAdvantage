import React from 'react';
import Enzyme, { shallow } from 'enzyme';

import Checkbox from './index';

test('checkbox without label renders correctly', () => {
    const component = shallow(<Checkbox />);

    expect(component).toMatchSnapshot();
});

test('checkbox with label renders correctly', () => {
    const component = shallow(
        <Checkbox
            label="test"
            id="test"
        />
    );

    expect(component).toMatchSnapshot();
});
