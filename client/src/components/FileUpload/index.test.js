import React from 'react';
import Enzyme, { shallow } from 'enzyme';

import FileUpload from './index';

it('renders correctly', () => {
    const component = shallow(<FileUpload />);

    expect(component).toMatchSnapshot();
});
