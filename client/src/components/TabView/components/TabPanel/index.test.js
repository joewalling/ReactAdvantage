import React from 'react';
import Enzyme, { shallow } from 'enzyme';

import TabPanel from './index';

it('renders correctly', () => {
    const component = shallow(<TabPanel />);

    expect(component).toMatchSnapshot();
});
